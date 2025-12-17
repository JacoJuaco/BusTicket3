using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Boleto
    {
        [Key]
        public int BoletoId { get; set; }

        public int VentaId { get; set; }
        public Venta Venta { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        public int AsientoId { get; set; }
        public Asiento Asiento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public DateTime FechaCompra { get; set; }

        public string Codigo { get; set; }
    }
}
