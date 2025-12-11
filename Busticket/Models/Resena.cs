using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Resena
    {
        [Key]
        public int ResenaId { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("Ruta")]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
