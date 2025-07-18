using System.ComponentModel.DataAnnotations;

namespace Control.Models
{
    public class CategoriaMaterial
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Relación
        public virtual ICollection<Material> Materiales { get; set; } = new List<Material>();

        // Auditoría mínima
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
    }
}