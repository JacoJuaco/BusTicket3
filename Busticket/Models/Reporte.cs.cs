using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Reporte
    {
        [Key]
        public int ReporteId { get; set; }

        [ForeignKey("Bus")]
        public int BusId { get; set; }
        public Bus Bus { get; set; }

        [ForeignKey("Conductor")]
        public int ConductorId { get; set; }
        public Conductor Conductor { get; set; }

        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
