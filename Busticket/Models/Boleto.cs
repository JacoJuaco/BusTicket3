using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Boleto
    {
        [Key]
        public int BoletoId { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("Itinerario")]
        public int ItinerarioId { get; set; }
        public Itinerario Itinerario { get; set; }

        public string? Asiento { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
