using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Busticket.Models
{
    public class Boleto
    {
        [Key]
        public int BoletoId { get; set; }

        // Relación con IdentityUser (AspNetUsers)
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        // Relación con Itinerario
        public int ItinerarioId { get; set; }
        public Itinerario Itinerario { get; set; }

        public string? Asiento { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
