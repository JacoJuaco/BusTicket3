using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    [Table("Ruta")]
    public class Ruta
    {
        [Key]
        public int RutaId { get; set; }

        // Ciudad Origen
        [Required(ErrorMessage = "Debe seleccionar la ciudad de origen.")]
        public int CiudadOrigenId { get; set; }   // ← YA NO ES NULLABLE

        [ForeignKey("CiudadOrigenId")]
        public Ciudad CiudadOrigen { get; set; }  // ← YA NO ES NULLABLE

        // Ciudad Destino
        [Required(ErrorMessage = "Debe seleccionar la ciudad de destino.")]
        public int CiudadDestinoId { get; set; }  // ← YA NO ES NULLABLE

        [ForeignKey("CiudadDestinoId")]
        public Ciudad CiudadDestino { get; set; } // ← YA NO ES NULLABLE

        // Empresa
        [Required(ErrorMessage = "Debe seleccionar una empresa.")]
        public int EmpresaId { get; set; }        // ← YA NO ES NULLABLE

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }      // ← YA NO ES NULLABLE

        // Precio
        [Required(ErrorMessage = "Debe ingresar un precio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        // Duración
        [Required(ErrorMessage = "Debe ingresar la duración.")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a 0.")]
        public int DuracionMin { get; set; }

        public string? ImagenUrl { get; set; }

        public List<Asiento>? Asiento { get; set; }
    }
}
