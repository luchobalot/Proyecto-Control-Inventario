using AutoMapper;
using Control.Models;
using Control.Models.Dtos.DTOPersona;

namespace Control.ControlMapper.Profiles
{
    public class PersonaProfile : Profile
    {
        public PersonaProfile()
        {
            // ==================== MAPEOS DESDE ENTIDAD A DTO ====================

            // Mapeo de Persona a PersonaDTO (para detalles completos)
            CreateMap<Persona, PersonaDTO>()
                .ForMember(dest => dest.OficinaNumero, opt => opt.MapFrom(src => src.Oficina != null ? src.Oficina.Numero : (int?)null))
                .ForMember(dest => dest.OfficinaDepartamento, opt => opt.MapFrom(src => src.Oficina != null ? src.Oficina.Departamento : null))
                .ForMember(dest => dest.MaterialesAsignados, opt => opt.Ignore()) // Se calculará en el servicio
                .ForMember(dest => dest.FechaUltimaAsignacion, opt => opt.Ignore()); // Se calculará en el servicio

            // Mapeo de Persona a PersonaListDTO (para listados)
            CreateMap<Persona, PersonaListDTO>()
                .ForMember(dest => dest.OficinaNumero, opt => opt.MapFrom(src => src.Oficina != null ? src.Oficina.Numero : (int?)null))
                .ForMember(dest => dest.MaterialesAsignados, opt => opt.Ignore()); // Se calculará en el servicio

            // ==================== MAPEOS DESDE DTO A ENTIDAD ====================

            // Mapeo de CreatePersonaDTO a Persona (para creación)
            CreateMap<CreatePersonaDTO, Persona>()
                .ForMember(dest => dest.IdPersona, opt => opt.Ignore()) // Se genera automáticamente
                .ForMember(dest => dest.Oficina, opt => opt.Ignore()); // Se resolverá por la FK

            // Mapeo de UpdatePersonaDTO a Persona (para actualización)
            CreateMap<UpdatePersonaDTO, Persona>()
                .ForMember(dest => dest.Oficina, opt => opt.Ignore()); // Se resolverá por la FK

            // ==================== MAPEOS ADICIONALES ====================

            // Mapeo directo entre DTOs si es necesario
            CreateMap<PersonaDTO, PersonaListDTO>();

            // Mapeo inverso de PersonaDTO a Persona (útil para algunos casos)
            CreateMap<PersonaDTO, Persona>()
                .ForMember(dest => dest.Oficina, opt => opt.Ignore()); // Se resolverá por la FK

            // ==================== CONFIGURACIONES PERSONALIZADAS ====================

            // Configuración para manejar valores null en las propiedades de navegación
            CreateMap<Persona, PersonaDTO>()
                .ForMember(dest => dest.OficinaNumero, opt => opt.MapFrom(src => src.Oficina != null ? src.Oficina.Numero : (int?)null))
                .ForMember(dest => dest.OfficinaDepartamento, opt => opt.MapFrom(src => src.Oficina != null ? src.Oficina.Departamento : null))
                .ForMember(dest => dest.MaterialesAsignados, opt => opt.Ignore())
                .ForMember(dest => dest.FechaUltimaAsignacion, opt => opt.Ignore());

            // Configuración adicional para PersonaListDTO
            CreateMap<Persona, PersonaListDTO>()
                .ForMember(dest => dest.OficinaNumero, opt => opt.MapFrom(src => src.Oficina != null ? src.Oficina.Numero : (int?)null))
                .ForMember(dest => dest.MaterialesAsignados, opt => opt.Ignore());
        }
    }
}