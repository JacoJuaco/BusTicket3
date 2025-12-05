using Busticket.Data;
using Busticket.DTOs;
using Busticket.Extensions;
using Busticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Busticket.Controllers
{
    [Route("Carrito")]
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

  
        public class AgregarCarritoDto
        {
            public int RutaId { get; set; }
            public List<AsientoDto> Asientos { get; set; }
        }

        public class AsientoDto
        {
            public string Id { get; set; }
            public string Codigo { get; set; }
        }

        [HttpPost("Agregar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar([FromBody] AgregarCarritoDto dto)
        {
            if (dto == null || dto.Asientos == null || !dto.Asientos.Any())
                return BadRequest(new { mensaje = "No hay asientos seleccionados." });

            var ruta = _context.Rutas.FirstOrDefault(r => r.RutaId == dto.RutaId);
            if (ruta == null)
                return BadRequest(new { mensaje = "Ruta no encontrada." });

            decimal precio = ruta.Precio;

            
            var carrito = HttpContext.Session.GetObjectFromJson<List<string>>("Carrito") ?? new List<string>();

            foreach (var asiento in dto.Asientos)
            {
                if (!carrito.Contains(asiento.Codigo))
                    carrito.Add(asiento.Codigo);

                var venta = new Venta
                {
                    AsientoId = int.Parse(asiento.Id),
                    RutaId = dto.RutaId,
                    Fecha = DateTime.Now
                };
                _context.Ventas.Add(venta);
            }

            await _context.SaveChangesAsync();

    
            HttpContext.Session.SetObjectAsJson("Carrito", carrito);
            decimal total = carrito.Count * precio;
            HttpContext.Session.SetString("Total", total.ToString());

            return Ok(new
            {
                mensaje = "Asientos agregados",
                carritoCount = carrito.Count,
                total
            });
        }

    }
}
