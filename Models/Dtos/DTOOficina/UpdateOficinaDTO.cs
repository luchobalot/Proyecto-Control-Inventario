using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Control.Models.Dtos.DTOOficina
{
    public class UpdateOficinaDTO
    {
        [Required(ErrorMessage = "El ID es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser un número positivo")]
        [DefaultValue(1)]
        public int IdOficina { get; set; }

        [Required(ErrorMessage = "El número de oficina es requerido")]
        [Range(1, 150, ErrorMessage = "El número de oficina debe estar entre 1 y 150")]
        [DefaultValue(1)]
        public int Numero { get; set; }

        [StringLength(200, ErrorMessage = "El departamento no puede exceder los 200 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-\.]+$", ErrorMessage = "El departamento solo puede contener letras, espacios, guiones y puntos")]
        [DefaultValue("Departamento de Sistemas")]
        public string? Departamento { get; set; }
    }
}