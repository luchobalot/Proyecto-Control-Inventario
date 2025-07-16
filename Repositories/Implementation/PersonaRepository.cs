using Control.Data;
using Control.Models;
using Control.Enums;
using Control.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Repositories.Implementations
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================== OPERACIONES BÁSICAS CRUD ====================

        public async Task<Persona?> GetByIdAsync(int id)
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .FirstOrDefaultAsync(p => p.IdPersona == id);
        }

        public async Task<Persona?> GetByIdSimpleAsync(int id)
        {
            return await _context.Personas
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPersona == id);
        }

        public async Task<List<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Persona>> GetAllSimpleAsync()
        {
            return await _context.Personas
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Persona> CreateAsync(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();

            // Recargar la persona con la oficina para retornar completa
            return await GetByIdAsync(persona.IdPersona) ?? persona;
        }

        public async Task<Persona> UpdateAsync(Persona persona)
        {
            var existingPersona = await _context.Personas.FindAsync(persona.IdPersona);
            if (existingPersona != null)
            {
                _context.Entry(existingPersona).CurrentValues.SetValues(persona);
                await _context.SaveChangesAsync();
                return await GetByIdAsync(persona.IdPersona) ?? existingPersona;
            }

            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(persona.IdPersona) ?? persona;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
                return false;

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Personas.AnyAsync(p => p.IdPersona == id);
        }

        // ==================== OPERACIONES DE BÚSQUEDA ====================

        public async Task<Persona?> GetByUsernameAsync(string username)
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .FirstOrDefaultAsync(p => p.NombreUsuario == username);
        }

        public async Task<List<Persona>> SearchByNameAsync(string searchTerm)
        {
            var normalizedSearchTerm = searchTerm.ToLower().Trim();

            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => p.Nombre.ToLower().Contains(normalizedSearchTerm) ||
                           p.Apellido.ToLower().Contains(normalizedSearchTerm) ||
                           (p.Nombre + " " + p.Apellido).ToLower().Contains(normalizedSearchTerm))
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Persona>> GetByOfficeAsync(int officeId)
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => p.OficinaId == officeId)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Persona>> GetByRoleAsync(RolUsuario role)
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => p.Rol == role)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Persona>> GetByJerarquiaAsync(Jerarquia jerarquia)
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => p.Jerarquia == jerarquia)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Persona>> GetByJerarquiaRangeAsync(Jerarquia jerarquiaMin, Jerarquia jerarquiaMax)
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => p.Jerarquia >= jerarquiaMin && p.Jerarquia <= jerarquiaMax)
                .OrderBy(p => p.Jerarquia)
                .ThenBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        // ==================== OPERACIONES DE VALIDACIÓN ====================

        public async Task<bool> ExistsUsernameAsync(string username)
        {
            return await _context.Personas
                .AnyAsync(p => p.NombreUsuario == username);
        }

        public async Task<bool> ExistsUsernameAsync(string username, int excludeId)
        {
            return await _context.Personas
                .AnyAsync(p => p.NombreUsuario == username && p.IdPersona != excludeId);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            // Verificar si tiene materiales asignados
            var hasMaterials = await _context.Materiales
                .AnyAsync(m => m.PersonaAsignadaId == id);

            // Verificar si aparece en historial como usuario que registró
            var hasHistoryRecords = await _context.AsignacionHistorial
                .AnyAsync(ah => ah.UsuarioRegistroId == id);

            return !hasMaterials && !hasHistoryRecords;
        }

        public async Task<bool> OfficeExistsAsync(int officeId)
        {
            return await _context.Oficinas.AnyAsync(o => o.IdOficina == officeId);
        }

        // ==================== OPERACIONES CON MATERIALES ====================

        public async Task<int> GetMaterialCountAsync(int personaId)
        {
            return await _context.Materiales
                .CountAsync(m => m.PersonaAsignadaId == personaId);
        }

        public async Task<List<Persona>> GetPersonasWithMaterialsAsync()
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => _context.Materiales.Any(m => m.PersonaAsignadaId == p.IdPersona))
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Persona>> GetPersonasWithoutMaterialsAsync()
        {
            return await _context.Personas
                .Include(p => p.Oficina)
                .Where(p => !_context.Materiales.Any(m => m.PersonaAsignadaId == p.IdPersona))
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<DateTime?> GetLastMaterialAssignmentDateAsync(int personaId)
        {
            return await _context.AsignacionHistorial
                .Where(ah => ah.PersonaId == personaId)
                .OrderByDescending(ah => ah.FechaAsignacion)
                .Select(ah => ah.FechaAsignacion)
                .FirstOrDefaultAsync();
        }

        // ==================== OPERACIONES DE PAGINACIÓN ====================

        public async Task<(List<Persona> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Personas.Include(p => p.Oficina);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Persona> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            int? officeId = null,
            RolUsuario? role = null,
            Jerarquia? jerarquia = null)
        {
            var query = _context.Personas.Include(p => p.Oficina).AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var normalizedSearchTerm = searchTerm.ToLower().Trim();
                query = query.Where(p =>
                    p.Nombre.ToLower().Contains(normalizedSearchTerm) ||
                    p.Apellido.ToLower().Contains(normalizedSearchTerm) ||
                    (p.Nombre + " " + p.Apellido).ToLower().Contains(normalizedSearchTerm));
            }

            if (officeId.HasValue)
            {
                query = query.Where(p => p.OficinaId == officeId.Value);
            }

            if (role.HasValue)
            {
                query = query.Where(p => p.Rol == role.Value);
            }

            if (jerarquia.HasValue)
            {
                query = query.Where(p => p.Jerarquia == jerarquia.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Personas.CountAsync();
        }

        public async Task<Dictionary<RolUsuario, int>> GetCountByRoleAsync()
        {
            return await _context.Personas
                .GroupBy(p => p.Rol)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetCountByOfficeAsync()
        {
            return await _context.Personas
                .Where(p => p.OficinaId.HasValue)
                .GroupBy(p => p.OficinaId!.Value)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<Jerarquia, int>> GetCountByJerarquiaAsync()
        {
            return await _context.Personas
                .GroupBy(p => p.Jerarquia)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}