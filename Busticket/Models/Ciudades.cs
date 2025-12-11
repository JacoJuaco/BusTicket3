using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    [Table("Ciudad")]
    public class Ciudad
    {
        [Key]
        public int CiudadId { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}