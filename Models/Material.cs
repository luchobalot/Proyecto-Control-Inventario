using System.ComponentModel.DataAnnotations;
using Control.Enums;

namespace Control.Models
{
    public class Material
    {
        [Key]
        public int IdMaterial { get; set; }

        // DATOS BÁSICOS
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        [StringLength(100)]
        public string? NumeroSerie { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        // RELACIÓN CON CATEGORÍA (directa)
        [Required]
        public int CategoriaId { get; set; }
        public virtual CategoriaMaterial Categoria { get; set; }

        // ESTADO ACTUAL
        [Required]
        public EstadoMaterial Estado { get; set; }

        // FECHAS IMPORTANTES
        public DateTime FechaRegistroSistema { get; set; } = DateTime.Now; // Cuando se registró en el sistema
        public DateTime? FechaAsignacion { get; set; } // Cuando se asignó a una persona (nullable)

        // ASIGNACIÓN ACTUAL
        public int? PersonaAsignadaId { get; set; }
        public virtual Persona? PersonaAsignada { get; set; }

        // UBICACIÓN ACTUAL
        public int OficinaId { get; set; }
        public virtual Oficina Oficina { get; set; }

        // CAMPOS ADICIONALES
        [StringLength(50)]
        public string? Marca { get; set; } // "Dell", "HP", "Samsung", "IKEA"

        [StringLength(500)]
        public string? Observaciones { get; set; } // Notas adicionales

        // HISTORIAL DE ASIGNACIONES
        public virtual ICollection<AsignacionHistorial> Historial { get; set; } = new List<AsignacionHistorial>();

        // AUDITORÍA
        public DateTime? FechaModificacion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // PROPIEDADES CALCULADAS
        public string NombreCompleto => $"{Categoria?.Nombre} - {Nombre}";
        public bool EstaAsignado => PersonaAsignadaId.HasValue;
        public int DiasDesdeRegistro => (DateTime.Now - FechaRegistroSistema).Days;
        public int? DiasAsignado => FechaAsignacion.HasValue ? (DateTime.Now - FechaAsignacion.Value).Days : null;
    }
}