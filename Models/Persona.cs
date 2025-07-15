using System.ComponentModel.DataAnnotations;
using Control.Enums;

namespace Control.Models
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        public Jerarquia Jerarquia { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        public RolUsuario Rol { get; set; }

        // Clave foránea para Oficina
        public int? OficinaId { get; set; }

        // Propiedad de navegación
        public virtual Oficina? Oficina { get; set; }
    }
}