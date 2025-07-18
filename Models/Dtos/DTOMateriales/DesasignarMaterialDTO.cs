using System.ComponentModel.DataAnnotations;

namespace Control.Models.Dtos.DTOMateriales
{
    public class DesasignarMaterialDTO
    {
        [Required(ErrorMessage = "El ID del material es requerido")]
        public int MaterialId { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
