using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Control.Enums;

namespace Control.Models.Dtos.DTOPersona
{
    public class CreatePersonaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        [DefaultValue("Juan")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        [DefaultValue("Pérez")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La jerarquía es requerida")]
        [EnumDataType(typeof(Jerarquia), ErrorMessage = "La jerarquía especificada no es válida")]
        [DefaultValue(Jerarquia.TenienteDeCorbeta)]
        public Jerarquia Jerarquia { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "El nombre de usuario debe tener entre 8 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, puntos, guiones y guiones bajos")]
        [DefaultValue("juancito_perez")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [EnumDataType(typeof(RolUsuario), ErrorMessage = "El rol especificado no es válido")]
        [DefaultValue(RolUsuario.Usuario)]
        public RolUsuario Rol { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una oficina válida")]
        [DefaultValue(1)]
        public int? OficinaId { get; set; }
    }
}