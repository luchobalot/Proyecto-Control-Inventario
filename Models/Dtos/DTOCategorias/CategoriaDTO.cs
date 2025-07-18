using System.ComponentModel.DataAnnotations;

namespace Control.Models.Dtos.DTOCategoria
{
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public int MaterialesCount { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CreateCategoriaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }
    }

    public class UpdateCategoriaDTO
    {
        [Required(ErrorMessage = "El ID es requerido")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }
    }
    public class CategoriaSimpleDTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
    }
}