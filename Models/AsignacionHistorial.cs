using System.ComponentModel.DataAnnotations;
using Control.Enums;

namespace Control.Models
{
    public class AsignacionHistorial
    {
        [Key]
        public int IdAsignacionHistorial { get; set; }

        // Relación con Material
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        // Relación con Persona (puede ser null si no está asignado)
        public int? PersonaId { get; set; }
        public virtual Persona? Persona { get; set; }

        // Relación con Oficina
        public int OficinaId { get; set; }
        public virtual Oficina Oficina { get; set; }

        // Fechas del movimiento
        public DateTime FechaAsignacion { get; set; }
        public DateTime? FechaDesasignacion { get; set; }

        // Estado del material en este momento
        public EstadoMaterial Estado { get; set; }

        // Motivo del cambio
        [StringLength(500)]
        public string? Motivo { get; set; }

        // Observaciones adicionales
        [StringLength(1000)]
        public string? Observaciones { get; set; }

        // Quien registró el movimiento
        public int UsuarioRegistroId { get; set; }
        public virtual Persona UsuarioRegistro { get; set; }

        // Timestamp del registro
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}