using Control.Enums;
using System.ComponentModel.DataAnnotations;

namespace Control.Models.Dtos.DTOMateriales
{
    public class CreateMaterialDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder los 100 caracteres")]
        public string? Modelo { get; set; }

        [StringLength(100, ErrorMessage = "El número de serie no puede exceder los 100 caracteres")]
        public string? NumeroSerie { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida")]
        public int CategoriaId { get; set; }

        [StringLength(50, ErrorMessage = "La marca no puede exceder los 50 caracteres")]
        public string? Marca { get; set; }

        [Required(ErrorMessage = "La oficina es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una oficina válida")]
        public int OficinaId { get; set; }

        [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder los 300 caracteres")]
        public string? Observaciones { get; set; }

        // Estado por defecto será Disponible
        public EstadoMaterial Estado { get; set; } = EstadoMaterial.Disponible;
    }
}
