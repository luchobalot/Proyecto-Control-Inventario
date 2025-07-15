using System.ComponentModel.DataAnnotations;
using Control.Enums;

namespace Control.Models.Dtos.DTOPersona
{
    public class CreatePersonaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La jerarquía es requerida")]
        [EnumDataType(typeof(Jerarquia), ErrorMessage = "La jerarquía especificada no es válida")]
        public Jerarquia Jerarquia { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "El nombre de usuario debe tener entre 8 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, puntos, guiones y guiones bajos")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [EnumDataType(typeof(RolUsuario), ErrorMessage = "El rol especificado no es válido")]
        public RolUsuario Rol { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una oficina válida")]
        public int? OficinaId { get; set; }
    }
}