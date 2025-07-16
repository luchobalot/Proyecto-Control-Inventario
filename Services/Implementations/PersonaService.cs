using AutoMapper;
using Control.Models;
using Control.Models.Dtos.DTOPersona;
using Control.Enums;
using Control.Repositories.Interfaces;
using Control.Services.Interfaces;

namespace Control.Services.Implementations
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;

        public PersonaService(IPersonaRepository personaRepository, IMapper mapper)
        {
            _personaRepository = personaRepository;
            _mapper = mapper;
        }

        // ==================== OPERACIONES CRUD ====================

        public async Task<PersonaDTO?> GetByIdAsync(int id)
        {
            var persona = await _personaRepository.GetByIdAsync(id);
            if (persona == null) return null;

            var personaDto = _mapper.Map<PersonaDTO>(persona);

            // Calcular propiedades adicionales
            personaDto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(id);
            personaDto.FechaUltimaAsignacion = await _personaRepository.GetLastMaterialAssignmentDateAsync(id);

            return personaDto;
        }

        public async Task<List<PersonaListDTO>> GetAllAsync()
        {
            var personas = await _personaRepository.GetAllAsync();
            var personasDto = _mapper.Map<List<PersonaListDTO>>(personas);

            // Calcular materiales asignados para cada persona
            foreach (var dto in personasDto)
            {
                dto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(dto.IdPersona);
            }

            return personasDto;
        }

        public async Task<PersonaDTO> CreateAsync(CreatePersonaDTO createDto)
        {
            // Validaciones de negocio
            if (await _personaRepository.ExistsUsernameAsync(createDto.NombreUsuario))
            {
                throw new InvalidOperationException($"El nombre de usuario '{createDto.NombreUsuario}' ya existe.");
            }

            if (createDto.OficinaId.HasValue && !await _personaRepository.OfficeExistsAsync(createDto.OficinaId.Value))
            {
                throw new InvalidOperationException($"La oficina con ID {createDto.OficinaId} no existe.");
            }

            // Mapear y crear
            var persona = _mapper.Map<Persona>(createDto);
            var personaCreada = await _personaRepository.CreateAsync(persona);

            // Retornar DTO completo
            return await GetByIdAsync(personaCreada.IdPersona) ??
                   throw new InvalidOperationException("Error al crear la persona.");
        }

        public async Task<PersonaDTO> UpdateAsync(UpdatePersonaDTO updateDto)
        {
            // Verificar que la persona existe
            var personaExistente = await _personaRepository.GetByIdSimpleAsync(updateDto.IdPersona);
            if (personaExistente == null)
            {
                throw new InvalidOperationException($"La persona con ID {updateDto.IdPersona} no existe.");
            }

            // Validaciones de negocio
            if (await _personaRepository.ExistsUsernameAsync(updateDto.NombreUsuario, updateDto.IdPersona))
            {
                throw new InvalidOperationException($"El nombre de usuario '{updateDto.NombreUsuario}' ya existe.");
            }

            if (updateDto.OficinaId.HasValue && !await _personaRepository.OfficeExistsAsync(updateDto.OficinaId.Value))
            {
                throw new InvalidOperationException($"La oficina con ID {updateDto.OficinaId} no existe.");
            }

            // Mapear y actualizar
            var persona = _mapper.Map<Persona>(updateDto);
            var personaActualizada = await _personaRepository.UpdateAsync(persona);

            // Retornar DTO completo
            return await GetByIdAsync(personaActualizada.IdPersona) ??
                   throw new InvalidOperationException("Error al actualizar la persona.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Verificar que la persona existe
            if (!await _personaRepository.ExistsAsync(id))
            {
                throw new InvalidOperationException($"La persona con ID {id} no existe.");
            }

            // Verificar que se puede eliminar
            if (!await _personaRepository.CanDeleteAsync(id))
            {
                throw new InvalidOperationException(
                    "No se puede eliminar la persona porque tiene materiales asignados o aparece en el historial.");
            }

            return await _personaRepository.DeleteAsync(id);
        }

        // ==================== OPERACIONES DE BÚSQUEDA ====================

        public async Task<List<PersonaListDTO>> SearchAsync(string searchTerm)
        {
            var personas = await _personaRepository.SearchByNameAsync(searchTerm);
            var personasDto = _mapper.Map<List<PersonaListDTO>>(personas);

            // Calcular materiales asignados para cada persona
            foreach (var dto in personasDto)
            {
                dto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(dto.IdPersona);
            }

            return personasDto;
        }

        public async Task<List<PersonaListDTO>> GetByOfficeAsync(int officeId)
        {
            var personas = await _personaRepository.GetByOfficeAsync(officeId);
            var personasDto = _mapper.Map<List<PersonaListDTO>>(personas);

            // Calcular materiales asignados para cada persona
            foreach (var dto in personasDto)
            {
                dto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(dto.IdPersona);
            }

            return personasDto;
        }

        public async Task<List<PersonaListDTO>> GetByRoleAsync(RolUsuario role)
        {
            var personas = await _personaRepository.GetByRoleAsync(role);
            var personasDto = _mapper.Map<List<PersonaListDTO>>(personas);

            // Calcular materiales asignados para cada persona
            foreach (var dto in personasDto)
            {
                dto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(dto.IdPersona);
            }

            return personasDto;
        }

        // ==================== OPERACIONES DE VALIDACIÓN ====================

        public async Task<bool> IsUsernameAvailableAsync(string username, int? excludeId = null)
        {
            return excludeId.HasValue
                ? !await _personaRepository.ExistsUsernameAsync(username, excludeId.Value)
                : !await _personaRepository.ExistsUsernameAsync(username);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return await _personaRepository.CanDeleteAsync(id);
        }

        public async Task<bool> OfficeExistsAsync(int officeId)
        {
            return await _personaRepository.OfficeExistsAsync(officeId);
        }

        // ==================== OPERACIONES DE PAGINACIÓN ====================

        public async Task<(List<PersonaListDTO> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            int? officeId = null,
            RolUsuario? role = null)
        {
            var (personas, totalCount) = await _personaRepository.GetPagedAsync(
                pageNumber, pageSize, searchTerm, officeId, role);

            var personasDto = _mapper.Map<List<PersonaListDTO>>(personas);

            // Calcular materiales asignados para cada persona
            foreach (var dto in personasDto)
            {
                dto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(dto.IdPersona);
            }

            return (personasDto, totalCount);
        }

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================

        public async Task<Dictionary<RolUsuario, int>> GetCountByRoleAsync()
        {
            return await _personaRepository.GetCountByRoleAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _personaRepository.GetTotalCountAsync();
        }
    }
}