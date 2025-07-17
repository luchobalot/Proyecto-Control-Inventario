using Control.Models;

namespace Control.Repositories.Interfaces
{
    public interface IOficinaRepository
    {
        // ==================== OPERACIONES BÁSICAS CRUD ====================
        Task<Oficina?> GetByIdAsync(int id);
        Task<Oficina?> GetByIdSimpleAsync(int id);
        Task<List<Oficina>> GetAllAsync();
        Task<List<Oficina>> GetAllSimpleAsync();
        Task<Oficina> CreateAsync(Oficina oficina);
        Task<Oficina> UpdateAsync(Oficina oficina);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // ==================== OPERACIONES DE BÚSQUEDA ====================
        Task<Oficina?> GetByNumberAsync(int numero);
        Task<List<Oficina>> SearchByDepartmentAsync(string searchTerm);
        Task<List<Oficina>> GetByDepartmentAsync(string departamento);

        // ==================== OPERACIONES DE VALIDACIÓN ====================
        Task<bool> ExistsNumberAsync(int numero);
        Task<bool> ExistsNumberAsync(int numero, int excludeId);
        Task<bool> CanDeleteAsync(int id);

        // ==================== OPERACIONES CON PERSONAS ====================
        Task<int> GetPersonCountAsync(int oficinaId);
        Task<List<Oficina>> GetOfficesWithPersonsAsync();
        Task<List<Oficina>> GetOfficesWithoutPersonsAsync();

        // ==================== OPERACIONES CON MATERIALES ====================
        Task<int> GetMaterialCountAsync(int oficinaId);
        Task<List<Oficina>> GetOfficesWithMaterialsAsync();
        Task<List<Oficina>> GetOfficesWithoutMaterialsAsync();

        // ==================== OPERACIONES DE PAGINACIÓN ====================
        Task<(List<Oficina> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<(List<Oficina> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            bool? hasPersons = null,
            bool? hasMaterials = null);

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================
        Task<int> GetTotalCountAsync();
        Task<Dictionary<string, int>> GetCountByDepartmentAsync();
        Task<(int WithPersons, int WithoutPersons)> GetPersonAssignmentStatsAsync();
        Task<(int WithMaterials, int WithoutMaterials)> GetMaterialLocationStatsAsync();
    }
}