using Control.Enums;

namespace Control.Models.Dtos.DTOPersona
{
    public class PersonaDTO
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

        // Información de la oficina
        public int? OficinaId { get; set; }
        public int? OficinaNumero { get; set; }
        public string? OfficinaDepartamento { get; set; }

        // Información adicional útil
        public int MaterialesAsignados { get; set; }
        public DateTime? FechaUltimaAsignacion { get; set; }
    }
}