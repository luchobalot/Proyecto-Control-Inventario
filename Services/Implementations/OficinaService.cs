using AutoMapper;
using Control.Models;
using Control.Models.Dtos.DTOOficina;
using Control.Repositories.Interfaces;
using Control.Services.Interfaces;

namespace Control.Services.Implementations
{
    public class OficinaService : IOficinaService
    {
        private readonly IOficinaRepository _oficinaRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;

        public OficinaService(IOficinaRepository oficinaRepository, IPersonaRepository personaRepository, IMapper mapper)
        {
            _oficinaRepository = oficinaRepository;
            _personaRepository = personaRepository;
            _mapper = mapper;
        }

        // ==================== OPERACIONES CRUD ====================

        public async Task<OficinaDTO?> GetByIdAsync(int id)
        {
            var oficina = await _oficinaRepository.GetByIdAsync(id);
            if (oficina == null) return null;

            var oficinaDto = _mapper.Map<OficinaDTO>(oficina);

            // Calcular propiedades adicionales
            oficinaDto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(id);
            oficinaDto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(id);

            // Poblar personas asignadas
            if (oficina.Personas != null && oficina.Personas.Any())
            {
                oficinaDto.PersonasAsignadas = _mapper.Map<List<PersonaOficinaDTO>>(oficina.Personas);

                // Calcular materiales asignados para cada persona
                foreach (var personaDto in oficinaDto.PersonasAsignadas)
                {
                    personaDto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(personaDto.IdPersona);
                }
            }

            // TODO: Poblar MaterialesUbicados cuando esté implementado el MaterialRepository

            return oficinaDto;
        }

        public async Task<OficinaDTO?> GetByNumberAsync(int numero)
        {
            var oficina = await _oficinaRepository.GetByNumberAsync(numero);
            if (oficina == null) return null;

            var oficinaDto = _mapper.Map<OficinaDTO>(oficina);

            // Calcular propiedades adicionales
            oficinaDto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(oficina.IdOficina);
            oficinaDto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(oficina.IdOficina);

            // Poblar personas asignadas
            if (oficina.Personas != null && oficina.Personas.Any())
            {
                oficinaDto.PersonasAsignadas = _mapper.Map<List<PersonaOficinaDTO>>(oficina.Personas);

                // Calcular materiales asignados para cada persona
                foreach (var personaDto in oficinaDto.PersonasAsignadas)
                {
                    personaDto.MaterialesAsignados = await _personaRepository.GetMaterialCountAsync(personaDto.IdPersona);
                }
            }

            return oficinaDto;
        }

        public async Task<List<OficinaListDTO>> GetAllAsync()
        {
            var oficinas = await _oficinaRepository.GetAllAsync();
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        public async Task<OficinaDTO> CreateAsync(CreateOficinaDTO createDto)
        {
            // Validaciones de negocio
            if (await _oficinaRepository.ExistsNumberAsync(createDto.Numero))
            {
                throw new InvalidOperationException($"Ya existe una oficina con el número {createDto.Numero}.");
            }

            // Mapear y crear
            var oficina = _mapper.Map<Oficina>(createDto);
            var oficinaCreada = await _oficinaRepository.CreateAsync(oficina);

            // Retornar DTO completo
            return await GetByIdAsync(oficinaCreada.IdOficina) ??
                   throw new InvalidOperationException("Error al crear la oficina.");
        }

        public async Task<OficinaDTO> UpdateAsync(UpdateOficinaDTO updateDto)
        {
            // Verificar que la oficina existe
            var oficinaExistente = await _oficinaRepository.GetByIdSimpleAsync(updateDto.IdOficina);
            if (oficinaExistente == null)
            {
                throw new InvalidOperationException($"La oficina con ID {updateDto.IdOficina} no existe.");
            }

            // Validaciones de negocio
            if (await _oficinaRepository.ExistsNumberAsync(updateDto.Numero, updateDto.IdOficina))
            {
                throw new InvalidOperationException($"Ya existe otra oficina con el número {updateDto.Numero}.");
            }

            // Mapear y actualizar
            var oficina = _mapper.Map<Oficina>(updateDto);
            var oficinaActualizada = await _oficinaRepository.UpdateAsync(oficina);

            // Retornar DTO completo
            return await GetByIdAsync(oficinaActualizada.IdOficina) ??
                   throw new InvalidOperationException("Error al actualizar la oficina.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Verificar que la oficina existe
            if (!await _oficinaRepository.ExistsAsync(id))
            {
                throw new InvalidOperationException($"La oficina con ID {id} no existe.");
            }

            // Verificar que se puede eliminar
            if (!await _oficinaRepository.CanDeleteAsync(id))
            {
                throw new InvalidOperationException(
                    "No se puede eliminar la oficina porque tiene personas asignadas, materiales ubicados o aparece en el historial.");
            }

            return await _oficinaRepository.DeleteAsync(id);
        }

        // ==================== OPERACIONES DE BÚSQUEDA ====================

        public async Task<List<OficinaListDTO>> SearchAsync(string searchTerm)
        {
            var oficinas = await _oficinaRepository.SearchByDepartmentAsync(searchTerm);
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        public async Task<List<OficinaListDTO>> GetByDepartmentAsync(string departamento)
        {
            var oficinas = await _oficinaRepository.GetByDepartmentAsync(departamento);
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        // ==================== OPERACIONES DE VALIDACIÓN ====================

        public async Task<bool> IsNumberAvailableAsync(int numero, int? excludeId = null)
        {
            return excludeId.HasValue
                ? !await _oficinaRepository.ExistsNumberAsync(numero, excludeId.Value)
                : !await _oficinaRepository.ExistsNumberAsync(numero);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return await _oficinaRepository.CanDeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _oficinaRepository.ExistsAsync(id);
        }

        // ==================== OPERACIONES DE FILTRADO ====================

        public async Task<List<OficinaListDTO>> GetOfficesWithPersonsAsync()
        {
            var oficinas = await _oficinaRepository.GetOfficesWithPersonsAsync();
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        public async Task<List<OficinaListDTO>> GetOfficesWithoutPersonsAsync()
        {
            var oficinas = await _oficinaRepository.GetOfficesWithoutPersonsAsync();
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        public async Task<List<OficinaListDTO>> GetOfficesWithMaterialsAsync()
        {
            var oficinas = await _oficinaRepository.GetOfficesWithMaterialsAsync();
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        public async Task<List<OficinaListDTO>> GetOfficesWithoutMaterialsAsync()
        {
            var oficinas = await _oficinaRepository.GetOfficesWithoutMaterialsAsync();
            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return oficinasDto;
        }

        // ==================== OPERACIONES DE PAGINACIÓN ====================

        public async Task<(List<OficinaListDTO> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            bool? hasPersons = null,
            bool? hasMaterials = null)
        {
            var (oficinas, totalCount) = await _oficinaRepository.GetPagedAsync(
                pageNumber, pageSize, searchTerm, hasPersons, hasMaterials);

            var oficinasDto = _mapper.Map<List<OficinaListDTO>>(oficinas);

            // Calcular contadores para cada oficina
            foreach (var dto in oficinasDto)
            {
                dto.CantidadPersonas = await _oficinaRepository.GetPersonCountAsync(dto.IdOficina);
                dto.CantidadMateriales = await _oficinaRepository.GetMaterialCountAsync(dto.IdOficina);
            }

            return (oficinasDto, totalCount);
        }

        // ==================== OPERACIONES DE ESTADÍSTICAS ====================

        public async Task<Dictionary<string, int>> GetCountByDepartmentAsync()
        {
            return await _oficinaRepository.GetCountByDepartmentAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _oficinaRepository.GetTotalCountAsync();
        }

        public async Task<(int WithPersons, int WithoutPersons)> GetPersonAssignmentStatsAsync()
        {
            return await _oficinaRepository.GetPersonAssignmentStatsAsync();
        }

        public async Task<(int WithMaterials, int WithoutMaterials)> GetMaterialLocationStatsAsync()
        {
            return await _oficinaRepository.GetMaterialLocationStatsAsync();
        }

        // ==================== OPERACIONES PARA DROPDOWNS/SELECTS ====================

        public async Task<List<OficinaListDTO>> GetForSelectAsync()
        {
            var oficinas = await _oficinaRepository.GetAllSimpleAsync();
            return _mapper.Map<List<OficinaListDTO>>(oficinas);
        }
    }
}