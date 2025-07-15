using Control.Enums;

namespace Control.Models.Dtos.DTOPersona
{
    public class PersonaListDTO
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public Jerarquia Jerarquia { get; set; }
        public string JerarquiaDescripcion => Jerarquia.ToString();
        public string NombreUsuario { get; set; }
        public RolUsuario Rol { get; set; }
        public string RolDescripcion => Rol.ToString();

        // Solo información básica de la oficina
        public int? OficinaId { get; set; }
        public int? OficinaNumero { get; set; }

        // Información resumida para listados
        public int MaterialesAsignados { get; set; }
        public bool TieneMaterialesAsignados => MaterialesAsignados > 0;
    }
}