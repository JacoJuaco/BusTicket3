using System.ComponentModel.DataAnnotations;

namespace Busticket.Models
{
    public class Conductor
    {
        [Key]
        public int ConductorId { get; set; }
        public string? Nombre { get; set; }
        public string? Licencia { get; set; }
        public string? Telefono { get; set; }
    }
}
