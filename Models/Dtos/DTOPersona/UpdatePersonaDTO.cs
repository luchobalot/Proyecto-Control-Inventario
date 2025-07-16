using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Control.Enums;

namespace Control.Models.Dtos.DTOPersona
{
    public class UpdatePersonaDTO
    {
        [Required(ErrorMessage = "El ID es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser un número positivo")]
        [DefaultValue("1")]
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        [DefaultValue("Juan")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        [DefaultValue("Juan")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La jerarquía es requerida")]
        [EnumDataType(typeof(Jerarquia), ErrorMessage = "La jerarquía especificada no es válida")]
        [DefaultValue("0")]
        public Jerarquia Jerarquia { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres")]
        [DefaultValue("juan_perez")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, puntos, guiones y guiones bajos")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [EnumDataType(typeof(RolUsuario), ErrorMessage = "El rol especificado no es válido")]
        [DefaultValue("1")]
        public RolUsuario Rol { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una oficina válida")]
        [DefaultValue("1")]
        public int? OficinaId { get; set; }
    }
}