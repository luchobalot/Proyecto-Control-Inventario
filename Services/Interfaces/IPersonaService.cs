using Control.Models.Dtos.DTOPersona;
using Control.Enums;

namespace Control.Services.Interfaces
{
    public interface IPersonaService
    {
        // ==================== OPERACIONES CRUD ====================

        /// Obtiene una persona por ID con información completa
        Task<PersonaDTO?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las personas para listado
        /// </summary>
        Task<List<PersonaListDTO>> GetAllAsync();

        /// <summary>
        /// Crea una nueva persona
        /// </summary>
        Task<PersonaDTO> CreateAsync(CreatePersonaDTO createDto);

        /// <summary>
        /// Actualiza una persona existente
        /// </summary>
        Task<PersonaDTO> UpdateAsync(UpdatePersonaDTO updateDto);

        /// <summary>
        /// Elimina una persona
        /// </summary>
        Task<bool> DeleteAsync(int id);

        // ==================== OPERACIONES DE BÚSQUEDA ====================

        /// <summary>
        /// Busca personas por nombre o apellido
        /// </summary>
        Task<List<PersonaListDTO>> SearchAsync(string searchTerm);

        /// <summary>
        /// Obtiene personas por oficina
        /// </summary>
        Task<List<PersonaListDTO>> GetByOfficeAsync(int officeId);

        /// <summary>
        /// Obtiene personas por rol
        /// </summary>
        Task<List<PersonaListDTO>> GetByRoleAsync(RolUsuario role);

        // ==================== OPERACIONES DE VALIDACIÓN ====================

        /// <summary>
        /// Verifica si un username está disponible
        /// </summary>
        Task<bool> IsUsernameAvailableAsync(string username, int? excludeId = null);

        /// <summary>
        /// Verifica si una persona puede ser eliminada
        /// </summary>
        Task<bool> CanDeleteAsync(int id);

        /// <summary>
        /// Verifica si una oficina existe
        /// </summary>
        Task<bool> OfficeExistsAsync(int officeId);

        // ==================== OPERACIONES DE PAGINACIÓN ====================

        /// <summary>
        /// Obtiene personas con paginación
        /// </summary>
        Task<(List<PersonaListDTO> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            int? officeId = null,
            RolUsuario? role = null);

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================

        /// <summary>
        /// Obtiene conteo de personas por rol
        /// </summary>
        Task<Dictionary<RolUsuario, int>> GetCountByRoleAsync();

        /// <summary>
        /// Obtiene conteo total de personas
        /// </summary>
        Task<int> GetTotalCountAsync();
    }
}