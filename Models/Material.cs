using System.ComponentModel.DataAnnotations;
using Control.Enums;

namespace Control.Models
{
    public class Material
    {
        [Key]
        public int IdMaterial { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } // Código único del equipo

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        [StringLength(100)]
        public string? NumeroSerie { get; set; }

        [Required]
        public TipoMaterial Tipo { get; set; }

        [Required]
        public EstadoMaterial Estado { get; set; }

        public DateTime FechaAdquisicion { get; set; }
        public decimal? Precio { get; set; }

        // Asignación actual
        public int? PersonaAsignadaId { get; set; }
        public virtual Persona? PersonaAsignada { get; set; }

        // Oficina actual
        public int OficinaId { get; set; }
        public virtual Oficina Oficina { get; set; }

        // Historial completo
        public virtual ICollection<AsignacionHistorial> Historial { get; set; } = new List<AsignacionHistorial>();

        // Auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
    }
}