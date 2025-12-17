using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Venta
    {
        public int VentaId { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        // 💰 Total pagado por TODA la compra
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }

        // 🧾 Una venta tiene muchos boletos
        public ICollection<Boleto> Boletos { get; set; }
    }
}
