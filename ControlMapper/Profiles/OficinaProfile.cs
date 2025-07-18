using AutoMapper;
using Control.Models;
using Control.Models.Dtos.DTOOficina;

namespace Control.ControlMapper.Profiles
{
    public class OficinaProfile : Profile
    {
        public OficinaProfile()
        {
            // ==================== MAPEOS DESDE ENTIDAD A DTO ====================

            // Mapeo de Oficina a OficinaDTO (para detalles completos)
            CreateMap<Oficina, OficinaDTO>()
                .ForMember(dest => dest.CantidadPersonas, opt => opt.Ignore()) // Se calculará en el servicio
                .ForMember(dest => dest.CantidadMateriales, opt => opt.Ignore()) // Se calculará en el servicio
                .ForMember(dest => dest.PersonasAsignadas, opt => opt.Ignore()) // Se calculará en el servicio
                .ForMember(dest => dest.MaterialesUbicados, opt => opt.Ignore()); // Se calculará en el servicio

            // Mapeo de Oficina a OficinaListDTO (para listados)
            CreateMap<Oficina, OficinaListDTO>()
                .ForMember(dest => dest.CantidadPersonas, opt => opt.Ignore()) // Se calculará en el servicio
                .ForMember(dest => dest.CantidadMateriales, opt => opt.Ignore()); // Se calculará en el servicio

            // ==================== MAPEOS DESDE DTO A ENTIDAD ====================

            // Mapeo de CreateOficinaDTO a Oficina (para creación)
            CreateMap<CreateOficinaDTO, Oficina>()
                .ForMember(dest => dest.IdOficina, opt => opt.Ignore()) // Se genera automáticamente
                .ForMember(dest => dest.Personas, opt => opt.Ignore()); // Se resolverá por la relación

            // Mapeo de UpdateOficinaDTO a Oficina (para actualización)
            CreateMap<UpdateOficinaDTO, Oficina>()
                .ForMember(dest => dest.Personas, opt => opt.Ignore()); // Se resolverá por la relación

            // ==================== MAPEOS ADICIONALES ====================

            // Mapeo directo entre DTOs si es necesario
            CreateMap<OficinaDTO, OficinaListDTO>();

            // Mapeo inverso de OficinaDTO a Oficina (útil para algunos casos)
            CreateMap<OficinaDTO, Oficina>()
                .ForMember(dest => dest.Personas, opt => opt.Ignore()); // Se resolverá por la relación

            // ==================== MAPEOS PARA DTOs ANIDADOS ====================

            // Mapeo de Persona a PersonaOficinaDTO (para mostrar en OficinaDTO)
            CreateMap<Persona, PersonaOficinaDTO>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"))
                .ForMember(dest => dest.JerarquiaDescripcion, opt => opt.MapFrom(src => src.Jerarquia.ToString()))
                .ForMember(dest => dest.MaterialesAsignados, opt => opt.Ignore()); // Se calculará en el servicio

            // Mapeo de Material a MaterialOficinaDTO (para mostrar en OficinaDTO) -- CORREGIR
            //CreateMap<Material, MaterialOficinaDTO>()
            //    .ForMember(dest => dest.TipoDescripcion, opt => opt.MapFrom(src => src.Tipo.ToString()))
            //    .ForMember(dest => dest.EstadoDescripcion, opt => opt.MapFrom(src => src.Estado.ToString()))
            //    .ForMember(dest => dest.PersonaAsignada, opt => opt.MapFrom(src =>
            //        src.PersonaAsignada != null ? $"{src.PersonaAsignada.Nombre} {src.PersonaAsignada.Apellido}" : null));
        }
    }
}