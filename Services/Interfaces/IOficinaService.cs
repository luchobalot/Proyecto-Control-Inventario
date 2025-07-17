using Control.Models.Dtos.DTOOficina;

namespace Control.Services.Interfaces
{
    public interface IOficinaService
    {
        // ==================== OPERACIONES CRUD ====================
        Task<OficinaDTO?> GetByIdAsync(int id);
        Task<List<OficinaListDTO>> GetAllAsync();
        Task<OficinaDTO> CreateAsync(CreateOficinaDTO createDto);
        Task<OficinaDTO> UpdateAsync(UpdateOficinaDTO updateDto);
        Task<bool> DeleteAsync(int id);

        // ==================== OPERACIONES DE BÚSQUEDA ====================
        Task<OficinaDTO?> GetByNumberAsync(int numero);
        Task<List<OficinaListDTO>> SearchAsync(string searchTerm);
        Task<List<OficinaListDTO>> GetByDepartmentAsync(string departamento);

        // ==================== OPERACIONES DE VALIDACIÓN ====================
        Task<bool> IsNumberAvailableAsync(int numero, int? excludeId = null);
        Task<bool> CanDeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // ==================== OPERACIONES DE FILTRADO ====================
        Task<List<OficinaListDTO>> GetOfficesWithPersonsAsync();
        Task<List<OficinaListDTO>> GetOfficesWithoutPersonsAsync();
        Task<List<OficinaListDTO>> GetOfficesWithMaterialsAsync();
        Task<List<OficinaListDTO>> GetOfficesWithoutMaterialsAsync();

        // ==================== OPERACIONES DE PAGINACIÓN ====================
        Task<(List<OficinaListDTO> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            bool? hasPersons = null,
            bool? hasMaterials = null);

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================
        Task<Dictionary<string, int>> GetCountByDepartmentAsync();
        Task<int> GetTotalCountAsync();
        Task<(int WithPersons, int WithoutPersons)> GetPersonAssignmentStatsAsync();
        Task<(int WithMaterials, int WithoutMaterials)> GetMaterialLocationStatsAsync();

        // ==================== OPERACIONES PARA DROPDOWNS/SELECTS ====================
        Task<List<OficinaListDTO>> GetForSelectAsync();
    }
}