using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
  // ============================
  // USUARIO
  // ============================
  public class Usuario
  {
    [Key]
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Telefono { get; set; }
    public string Password { get; set; } = null!;
  }

  // ============================
  // EMPRESA
  // ============================
  public class Empresa
  {
    [Key]
    public int EmpresaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Pais { get; set; }
    public string? Telefono { get; set; }
  }

  // ============================
  // BUS
  // ============================
  public class Bus
  {
    [Key]
    public int BusId { get; set; }

    [ForeignKey("Empresa")]
    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

    public string? Placa { get; set; }
    public string? Modelo { get; set; }
    public int Capacidad { get; set; }
  }

  // ============================
  // CONDUCTOR
  // ============================
  public class Conductor
  {
    [Key]
    public int ConductorId { get; set; }
    public string? Nombre { get; set; }
    public string? Licencia { get; set; }
    public string? Telefono { get; set; }
  }

  // ============================
  // RUTA
  // ============================
  public class Ruta
  {
    [Key]
    public int RutaId { get; set; }

    public string Origen { get; set; } = null!;
    public string Destino { get; set; } = null!;
    public string? Empresa{ get; set; } = null!;

    public int DuracionMin { get; set; }

    public decimal Precio { get; set; }

    public string? ImagenUrl { get; set; }

    public double? OrigenLat { get; set; }
    public double? OrigenLng { get; set; }
    public double? DestinoLat { get; set; }
    public double? DestinoLng { get; set; }

    public List<Asiento>? Asientos { get; set; }
  }

  // ============================
  // ITINERARIO
  // ============================
  public class Itinerario
  {
    [Key]
    public int ItinerarioId { get; set; }

    public int RutaId { get; set; }
    public Ruta Ruta { get; set; }

    public int BusId { get; set; }
    public Bus Bus { get; set; }

    public int ConductorId { get; set; }
    public Conductor Conductor { get; set; }

    public DateTime Fecha { get; set; }
    public TimeSpan HoraSalida { get; set; }
    public TimeSpan HoraLlegada { get; set; }
  }

  // ============================
  // ASIENTO
  // ============================
  public class Asiento
  {
    [Key]
    public int AsientoId { get; set; }
    public string Codigo { get; set; } = null!;
    public bool Disponible { get; set; }

    [ForeignKey("Ruta")]
    public int RutaId { get; set; }
    public Ruta Ruta { get; set; }
  }

  // ============================
  // VENTAS
  // ============================
  public class Venta
  {
    [Key]
    public int VentaId { get; set; }
    public int AsientoId { get; set; }
    public int RutaId { get; set; }
    public DateTime Fecha { get; set; }
  }

  // ============================
  // BOLETO
  // ============================
  public class Boleto
  {
    [Key]
    public int BoletoId { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int ItinerarioId { get; set; }
    public Itinerario Itinerario { get; set; }

    public string? Asiento { get; set; }
    public DateTime FechaCompra { get; set; }
  }

  // ============================
  // OFERTA
  // ============================
  public class Oferta
  {
    [Key]
    public int OfertaId { get; set; }

    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public int Descuento { get; set; }
    public bool Vigente { get; set; }
  }

  // ============================
  // RESENA
  // ============================
  public class Resena
  {
    [Key]
    public int ResenaId { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int RutaId { get; set; }
    public Ruta Ruta { get; set; }

    public int Calificacion { get; set; }
    public string? Comentario { get; set; }
    public DateTime Fecha { get; set; }
  }

  // ============================
  // REPORTE
  // ============================
  public class Reporte
  {
    [Key]
    public int ReporteId { get; set; }

    public int BusId { get; set; }
    public Bus Bus { get; set; }

    public int ConductorId { get; set; }
    public Conductor Conductor { get; set; }

    public string? Descripcion { get; set; }
    public DateTime Fecha { get; set; }
  }
}

