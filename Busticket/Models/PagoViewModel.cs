using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Busticket.Models
{
    public class PagoViewModel
    {
        // =========================
        // COMPRA (OBLIGATORIO)
        // =========================
        public int RutaId { get; set; }
        public List<string> Asientos { get; set; } = new();
        public decimal Total { get; set; }

        // =========================
        // SOLO PARA MOSTRAR
        // =========================
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Empresa { get; set; }
        public string Duracion { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }

        // =========================
        // TARJETA (FAKE / UI)
        // ❌ NO VALIDAR
        // =========================
        public string Nombre { get; set; }
        public string NumeroTarjeta { get; set; }
        public string Validez { get; set; }
        public string CVC { get; set; }

        public string Descuento { get; set; }
    }

}
