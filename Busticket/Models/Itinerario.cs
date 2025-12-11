using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Itinerario
    {
        [Key]
        public int ItinerarioId { get; set; }

        [ForeignKey("Ruta")]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [ForeignKey("Bus")]
        public int BusId { get; set; }
        public Bus Bus { get; set; }

        [ForeignKey("Conductor")]
        public int ConductorId { get; set; }
        public Conductor Conductor { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }
    }
}
