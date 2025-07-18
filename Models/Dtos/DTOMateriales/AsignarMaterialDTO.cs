using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace Control.Models.Dtos.DTOMateriales
{
    public class AsignarMaterialDTO
    {
        [Required(ErrorMessage = "El ID del material es requerido")]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "La persona es requerida")]
        public int PersonaId { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
