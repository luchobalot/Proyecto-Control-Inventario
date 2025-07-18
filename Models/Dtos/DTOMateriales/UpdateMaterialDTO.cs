using Control.Enums;
using System.ComponentModel.DataAnnotations;

namespace Control.Models.Dtos.DTOMateriales
{
    public class UpdateMaterialDTO
    {
        [Required(ErrorMessage = "El ID es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser un número positivo")]
        public int IdMaterial { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        [StringLength(100)]
        public string? NumeroSerie { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }

        [StringLength(50)]
        public string? Marca { get; set; }

        [Required(ErrorMessage = "La oficina es requerida")]
        public int OficinaId { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public EstadoMaterial Estado { get; set; }
    }
}
