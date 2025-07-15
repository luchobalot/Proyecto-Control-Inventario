using System.ComponentModel.DataAnnotations;

namespace Control.Models
{
    public class Oficina
    {
        [Key]
        public int IdOficina { get; set; }

        [Required]
        public int Numero { get; set; }

        [StringLength(200)]
        public string? Departamento { get; set; }

        public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
    }
}