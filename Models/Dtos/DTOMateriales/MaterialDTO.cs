using Control.Enums;

namespace Control.Models.Dtos.DTOMateriales
{
    public class MaterialDTO
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; }
        public string? Modelo { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public DateTime FechaRegistroSistema { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public EstadoMaterial Estado { get; set; }
        public string EstadoDescripcion => Estado.ToString();
        public string? Observaciones { get; set; }

        // Información de categoría
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }

        // Información de asignación actual
        public int? PersonaAsignadaId { get; set; }
        public string? PersonaAsignadaNombre { get; set; }
        public string? PersonaAsignadaApellido { get; set; }
        public string? PersonaAsignadaCompleto { get; set; }

        // Información de oficina
        public int OficinaId { get; set; }
        public int OficinaNumero { get; set; }
        public string? OficinaDepartamento { get; set; }

        // Propiedades calculadas
        public string NombreCompleto { get; set; }
        public bool EstaAsignado { get; set; }
        public int DiasDesdeRegistro { get; set; }
        public int? DiasAsignado { get; set; }
    }
}
