using System.ComponentModel.DataAnnotations;

namespace Control.Models
{
    public class TipoMaterial
    {
        [Key]
        public int IdTipoMaterial { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        // Relación con categoría
        [Required]
        public int CategoriaId { get; set; }
        public virtual CategoriaMaterial Categoria { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        // Orden dentro de la categoría
        public int Orden { get; set; } = 0;

        // Campos adicionales específicos del tipo
        [StringLength(200)]
        public string? Especificaciones { get; set; } // JSON con campos específicos

        // Indica si los usuarios pueden crear nuevos elementos de este tipo
        public bool PermiteCreacionUsuario { get; set; } = false;

        // Relación con materiales
        public virtual ICollection<Material> Materiales { get; set; } = new List<Material>();

        // Auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
    }
}