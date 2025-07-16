using Control.Models;
using Control.Enums;

namespace Control.Repositories.Interfaces
{
    public interface IPersonaRepository
    {
        // ==================== OPERACIONES BÁSICAS CRUD ====================

        /// Obtiene una persona por su ID, incluyendo la oficina
        Task<Persona?> GetByIdAsync(int id);

        /// Obtiene una persona por su ID sin incluir relaciones (más rápido)

        Task<Persona?> GetByIdSimpleAsync(int id);


        /// Obtiene todas las personas con sus oficinas
        Task<List<Persona>> GetAllAsync();

        /// Obtiene todas las personas sin relaciones (para listados simples)
        Task<List<Persona>> GetAllSimpleAsync();

        /// Crea una nueva persona
        Task<Persona> CreateAsync(Persona persona);

        /// Actualiza una persona existente
        Task<Persona> UpdateAsync(Persona persona);

        /// Elimina una persona por su ID
        Task<bool> DeleteAsync(int id);

        /// Verifica si una persona existe
        Task<bool> ExistsAsync(int id);

        // ==================== OPERACIONES DE BÚSQUEDA ====================

        /// Busca una persona por su nombre de usuario
        Task<Persona?> GetByUsernameAsync(string username);

        /// Busca personas por nombre o apellido (búsqueda parcial)
        Task<List<Persona>> SearchByNameAsync(string searchTerm);

        /// Obtiene personas de una oficina específica
        Task<List<Persona>> GetByOfficeAsync(int officeId);

        /// Obtiene personas por rol
        Task<List<Persona>> GetByRoleAsync(RolUsuario role);

        /// Obtiene personas por jerarquía
        Task<List<Persona>> GetByJerarquiaAsync(Jerarquia jerarquia);

        /// Obtiene personas dentro de un rango de jerarquías
        Task<List<Persona>> GetByJerarquiaRangeAsync(Jerarquia jerarquiaMin, Jerarquia jerarquiaMax);

        // ==================== OPERACIONES DE VALIDACIÓN ====================

        /// Verifica si un nombre de usuario ya existe
        Task<bool> ExistsUsernameAsync(string username);

        /// Verifica si un nombre de usuario ya existe, excluyendo un ID específico (para updates)
        Task<bool> ExistsUsernameAsync(string username, int excludeId);

        /// Verifica si una persona puede ser eliminada (no tiene materiales asignados)
        Task<bool> CanDeleteAsync(int id);

        /// <summary>
        /// Verifica si una oficina existe
        /// </summary>
        Task<bool> OfficeExistsAsync(int officeId);

        // ==================== OPERACIONES CON MATERIALES ====================

        /// <summary>
        /// Obtiene el conteo de materiales asignados a una persona
        /// </summary>
        Task<int> GetMaterialCountAsync(int personaId);

        /// <summary>
        /// Obtiene personas que tienen materiales asignados
        /// </summary>
        Task<List<Persona>> GetPersonasWithMaterialsAsync();

        /// <summary>
        /// Obtiene personas sin materiales asignados
        /// </summary>
        Task<List<Persona>> GetPersonasWithoutMaterialsAsync();

        /// <summary>
        /// Obtiene la fecha de la última asignación de material para una persona
        /// </summary>
        Task<DateTime?> GetLastMaterialAssignmentDateAsync(int personaId);

        // ==================== OPERACIONES DE PAGINACIÓN ====================

        /// <summary>
        /// Obtiene personas con paginación
        /// </summary>
        Task<(List<Persona> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Obtiene personas con paginación y filtros
        /// </summary>
        Task<(List<Persona> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            int? officeId = null,
            RolUsuario? role = null,
            Jerarquia? jerarquia = null);

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================

        /// <summary>
        /// Obtiene el conteo total de personas
        /// </summary>
        Task<int> GetTotalCountAsync();

        /// <summary>
        /// Obtiene el conteo de personas por rol
        /// </summary>
        Task<Dictionary<RolUsuario, int>> GetCountByRoleAsync();

        /// <summary>
        /// Obtiene el conteo de personas por oficina
        /// </summary>
        Task<Dictionary<int, int>> GetCountByOfficeAsync();

        /// <summary>
        /// Obtiene el conteo de personas por jerarquía
        /// </summary>
        Task<Dictionary<Jerarquia, int>> GetCountByJerarquiaAsync();
    }
}