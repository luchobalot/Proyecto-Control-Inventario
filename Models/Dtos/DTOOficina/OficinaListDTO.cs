namespace Control.Models.Dtos.DTOOficina
{
    public class OficinaListDTO
    {
        public int IdOficina { get; set; }
        public int Numero { get; set; }
        public string? Departamento { get; set; }

        // Información resumida para listados
        public int CantidadPersonas { get; set; }
        public int CantidadMateriales { get; set; }

        // Propiedades calculadas útiles para el frontend
        public bool TienePersonasAsignadas => CantidadPersonas > 0;
        public bool TieneMaterialesUbicados => CantidadMateriales > 0;

        // Descripción completa para mostrar en select/dropdown
        public string DescripcionCompleta => !string.IsNullOrEmpty(Departamento)
            ? $"Oficina {Numero} - {Departamento}"
            : $"Oficina {Numero}";
    }
}