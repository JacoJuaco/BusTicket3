using System.ComponentModel.DataAnnotations;

namespace Busticket.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Password { get; set; } = null!;
    }
}
