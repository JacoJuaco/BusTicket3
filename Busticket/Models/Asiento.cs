using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Asiento
    {
        [Key]
        public int AsientoId { get; set; }

        [Required]
        public int Numero { get; set; }  // Número del asiento

        public bool Disponible { get; set; } = true; // true si se puede reservar

        [Required]
        public int RutaId { get; set; }

        [ForeignKey("RutaId")]
        public Ruta Ruta { get; set; } = null!;
    }


}
