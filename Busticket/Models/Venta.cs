using System;
using System.ComponentModel.DataAnnotations;

namespace Busticket.Models
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }
        public int AsientoId { get; set; }
        public int RutaId { get; set; }
        public DateTime Fecha { get; set; }
    }
}

