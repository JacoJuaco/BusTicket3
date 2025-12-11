using Busticket.Models;
using Microsoft.AspNetCore.Identity;

public class Venta
{
    public int VentaId { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

    public int AsientoId { get; set; }
    public Asiento Asiento { get; set; }

    public int RutaId { get; set; }
    public Ruta Ruta { get; set; }

    public DateTime Fecha { get; set; }
}
