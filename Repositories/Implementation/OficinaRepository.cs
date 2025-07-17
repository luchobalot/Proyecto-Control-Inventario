using Control.Data;
using Control.Models;
using Control.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Repositories.Implementations
{
    public class OficinaRepository : IOficinaRepository
    {
        private readonly ApplicationDbContext _context;

        public OficinaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================== OPERACIONES BÁSICAS CRUD ====================

        public async Task<Oficina?> GetByIdAsync(int id)
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .FirstOrDefaultAsync(o => o.IdOficina == id);
        }

        public async Task<Oficina?> GetByIdSimpleAsync(int id)
        {
            return await _context.Oficinas
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.IdOficina == id);
        }

        public async Task<List<Oficina>> GetAllAsync()
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        public async Task<List<Oficina>> GetAllSimpleAsync()
        {
            return await _context.Oficinas
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        public async Task<Oficina> CreateAsync(Oficina oficina)
        {
            _context.Oficinas.Add(oficina);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(oficina.IdOficina) ?? oficina;
        }

        public async Task<Oficina> UpdateAsync(Oficina oficina)
        {
            var existingOficina = await _context.Oficinas.FindAsync(oficina.IdOficina);
            if (existingOficina != null)
            {
                _context.Entry(existingOficina).CurrentValues.SetValues(oficina);
                await _context.SaveChangesAsync();
                return await GetByIdAsync(oficina.IdOficina) ?? existingOficina;
            }

            _context.Oficinas.Add(oficina);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(oficina.IdOficina) ?? oficina;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var oficina = await _context.Oficinas.FindAsync(id);
            if (oficina == null)
                return false;

            _context.Oficinas.Remove(oficina);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Oficinas.AnyAsync(o => o.IdOficina == id);
        }

        // ==================== OPERACIONES DE BÚSQUEDA ====================

        public async Task<Oficina?> GetByNumberAsync(int numero)
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .FirstOrDefaultAsync(o => o.Numero == numero);
        }

        public async Task<List<Oficina>> SearchByDepartmentAsync(string searchTerm)
        {
            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            return await _context.Oficinas
                .Include(o => o.Personas)
                .Where(o => o.Departamento != null && o.Departamento.ToLower().Contains(normalizedSearchTerm))
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        public async Task<List<Oficina>> GetByDepartmentAsync(string departamento)
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .Where(o => o.Departamento == departamento)
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        // ==================== OPERACIONES DE VALIDACIÓN ====================

        public async Task<bool> ExistsNumberAsync(int numero)
        {
            return await _context.Oficinas
                .AnyAsync(o => o.Numero == numero);
        }

        public async Task<bool> ExistsNumberAsync(int numero, int excludeId)
        {
            return await _context.Oficinas
                .AnyAsync(o => o.Numero == numero && o.IdOficina != excludeId);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            var hasPersons = await _context.Personas
                .AnyAsync(p => p.OficinaId == id);

            var hasMaterials = await _context.Materiales
                .AnyAsync(m => m.OficinaId == id);

            var hasHistoryRecords = await _context.AsignacionHistorial
                .AnyAsync(ah => ah.OficinaId == id);

            return !hasPersons && !hasMaterials && !hasHistoryRecords;
        }

        // ==================== OPERACIONES CON PERSONAS ====================

        public async Task<int> GetPersonCountAsync(int oficinaId)
        {
            return await _context.Personas
                .CountAsync(p => p.OficinaId == oficinaId);
        }

        public async Task<List<Oficina>> GetOfficesWithPersonsAsync()
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .Where(o => _context.Personas.Any(p => p.OficinaId == o.IdOficina))
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        public async Task<List<Oficina>> GetOfficesWithoutPersonsAsync()
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .Where(o => !_context.Personas.Any(p => p.OficinaId == o.IdOficina))
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        // ==================== OPERACIONES CON MATERIALES ====================

        public async Task<int> GetMaterialCountAsync(int oficinaId)
        {
            return await _context.Materiales
                .CountAsync(m => m.OficinaId == oficinaId);
        }

        public async Task<List<Oficina>> GetOfficesWithMaterialsAsync()
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .Where(o => _context.Materiales.Any(m => m.OficinaId == o.IdOficina))
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        public async Task<List<Oficina>> GetOfficesWithoutMaterialsAsync()
        {
            return await _context.Oficinas
                .Include(o => o.Personas)
                .Where(o => !_context.Materiales.Any(m => m.OficinaId == o.IdOficina))
                .OrderBy(o => o.Numero)
                .ToListAsync();
        }

        // ==================== OPERACIONES DE PAGINACIÓN ====================

        public async Task<(List<Oficina> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Oficinas.Include(o => o.Personas);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(o => o.Numero)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Oficina> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            bool? hasPersons = null,
            bool? hasMaterials = null)
        {
            var query = _context.Oficinas.Include(o => o.Personas).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var normalizedSearchTerm = searchTerm.ToLower().Trim();
                query = query.Where(o =>
                    o.Numero.ToString().Contains(normalizedSearchTerm) ||
                    (o.Departamento != null && o.Departamento.ToLower().Contains(normalizedSearchTerm)));
            }

            if (hasPersons.HasValue)
            {
                if (hasPersons.Value)
                {
                    query = query.Where(o => _context.Personas.Any(p => p.OficinaId == o.IdOficina));
                }
                else
                {
                    query = query.Where(o => !_context.Personas.Any(p => p.OficinaId == o.IdOficina));
                }
            }

            if (hasMaterials.HasValue)
            {
                if (hasMaterials.Value)
                {
                    query = query.Where(o => _context.Materiales.Any(m => m.OficinaId == o.IdOficina));
                }
                else
                {
                    query = query.Where(o => !_context.Materiales.Any(m => m.OficinaId == o.IdOficina));
                }
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(o => o.Numero)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Oficinas.CountAsync();
        }

        public async Task<Dictionary<string, int>> GetCountByDepartmentAsync()
        {
            return await _context.Oficinas
                .Where(o => o.Departamento != null)
                .GroupBy(o => o.Departamento!)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<(int WithPersons, int WithoutPersons)> GetPersonAssignmentStatsAsync()
        {
            var withPersons = await _context.Oficinas
                .CountAsync(o => _context.Personas.Any(p => p.OficinaId == o.IdOficina));

            var total = await _context.Oficinas.CountAsync();
            var withoutPersons = total - withPersons;

            return (withPersons, withoutPersons);
        }

        public async Task<(int WithMaterials, int WithoutMaterials)> GetMaterialLocationStatsAsync()
        {
            var withMaterials = await _context.Oficinas
                .CountAsync(o => _context.Materiales.Any(m => m.OficinaId == o.IdOficina));

            var total = await _context.Oficinas.CountAsync();
            var withoutMaterials = total - withMaterials;

            return (withMaterials, withoutMaterials);
        }
    }
}