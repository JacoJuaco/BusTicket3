using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Oferta
    {
        [Key]
        public int OfertaId { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public int Descuento { get; set; }
        public bool Vigente { get; set; }
    }
}
