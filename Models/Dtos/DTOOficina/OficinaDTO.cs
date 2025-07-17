namespace Control.Models.Dtos.DTOOficina
{
    public class OficinaDTO
    {
        public int IdOficina { get; set; }
        public int Numero { get; set; }
        public string? Departamento { get; set; }

        // Información adicional útil
        public int CantidadPersonas { get; set; }
        public int CantidadMateriales { get; set; }

        // Información de las personas asignadas (resumen)
        public List<PersonaOficinaDTO> PersonasAsignadas { get; set; } = new List<PersonaOficinaDTO>();

        // Información de los materiales ubicados (resumen)
        public List<MaterialOficinaDTO> MaterialesUbicados { get; set; } = new List<MaterialOficinaDTO>();
    }

    // DTO anidado para información básica de personas en la oficina
    public class PersonaOficinaDTO
    {
        public int IdPersona { get; set; }
        public string NombreCompleto { get; set; }
        public string JerarquiaDescripcion { get; set; }
        public int MaterialesAsignados { get; set; }
    }

    // DTO anidado para información básica de materiales en la oficina
    public class MaterialOficinaDTO
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string TipoDescripcion { get; set; }
        public string EstadoDescripcion { get; set; }
        public string? PersonaAsignada { get; set; }
    }
}