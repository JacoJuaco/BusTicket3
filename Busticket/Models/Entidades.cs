using Busticket.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busticket.Models
{
    public class Usuario
    {
        [Column("UsuarioId")]
        public int Id { get; set; }  // EF mapeará a UsuarioId
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Password { get; set; } = null!;
    }

    public class Asiento
    {
        public int AsientoId { get; set; }
        public string Codigo { get; set; }
        public bool Disponible { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
      
    }

    public class Venta
    {
        public int VentaId { get; set; }
        public int AsientoId { get; set; }
        public int RutaId { get; set; }
        public DateTime Fecha { get; set; }
     
    }


    public class CarritoItem
    {
        public int RutaId { get; set; }
        public int AsientoId { get; set; }
        public string Codigo { get; set; }
        public decimal Precio { get; set; }
    }


    public class Ruta
        {
            public int RutaId { get; set; }
            public string Origen { get; set; }
            public string Destino { get; set; }

            [Column("DuracionMin")]
            public int Duracion { get; set; }
       
        public decimal Precio { get; set; }
             public string? ImagenUrl { get; set; }


        // Coordenadas
        public double? OrigenLat { get; set; }
        public double? OrigenLng { get; set; }
        public double? DestinoLat { get; set; }
        public double? DestinoLng { get; set; }
        public List<Asiento> Asientos { get; set; }
        public string Empresa { get; set; }

    }




    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
    }

    public class Bus
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public int Capacidad { get; set; }
        public string Modelo { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }

    public class Conductor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Licencia { get; set; }
        public string Telefono { get; set; }
    }

    public class Itinerario
    {
        public int Id { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
        public int BusId { get; set; }
        public Bus Bus { get; set; }
        public int ConductorId { get; set; }
        public Conductor Conductor { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
    }

    public class Boleto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ItinerarioId { get; set; }
        public Itinerario Itinerario { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCompra { get; set; }
    }

    public class Oferta
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Descuento { get; set; } 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class Resena
    {
        public int ResenaId { get; set; } 
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        public int Calificacion { get; set; } 
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now; 
    }
    public class Reporte
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
