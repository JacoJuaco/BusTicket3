using System;
using System.Collections.Generic;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Busticket.Models
{
    public class PagoViewModel
    {
        public int RutaId { get; set; }


        public List<string> Asientos { get; set; } = new List<string>();

        public decimal Total { get; set; }

        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Empresa { get; set; }
        public string Duracion { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }

        // Datos de tarjeta para procesar el pago
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string NumeroTarjeta { get; set; }

        [Required]
        public string Validez { get; set; }

        [Required]
        public string CVC { get; set; }

        public string Descuento { get; set; }
    }
}
