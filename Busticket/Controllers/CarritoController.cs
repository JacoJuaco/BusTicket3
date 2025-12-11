using Busticket.Data;
using Busticket.DTOs;
using Busticket.Extensions;
using Busticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Busticket.Controllers
{
    [Route("Carrito")]
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarritoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class AgregarCarritoDto
        {
            public int RutaId { get; set; }
            public List<AsientoDto> Asientos { get; set; }
        }

        public class AsientoDto
        {
            public int Id { get; set; }
            public string Codigo { get; set; }
        }

        [HttpPost("Agregar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar([FromBody] AgregarCarritoDto dto)
        {
            if (dto == null || dto.Asientos == null || !dto.Asientos.Any())
                return BadRequest(new { mensaje = "No hay asientos seleccionados." });

            // Obtener la ruta
            var ruta = await _context.Ruta
                .FirstOrDefaultAsync(r => r.RutaId == dto.RutaId);

            if (ruta == null)
                return BadRequest(new { mensaje = "Ruta no encontrada." });

            // 🔥 Obtener usuario actual
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized(new { mensaje = "Debe iniciar sesión para comprar." });

            decimal precio = ruta.Precio;

            var carrito = HttpContext.Session.GetObjectFromJson<List<string>>("Carrito")
                          ?? new List<string>();

            foreach (var asiento in dto.Asientos)
            {
                // Agregar código al carrito
                if (!carrito.Contains(asiento.Codigo))
                    carrito.Add(asiento.Codigo);

                // Obtener asiento REAL de la BD
                var asientoDb = await _context.Asiento.FindAsync(asiento.Id);
                if (asientoDb == null)
                    return BadRequest(new { mensaje = $"Asiento {asiento.Id} no existe." });

                // Marcarlo como no disponible
                asientoDb.Disponible = false;

                // 🔥 Crear venta COMPLETA con UserId y EmpresaId
                var venta = new Venta
                {
                    UserId = userId,               // obligatorio
                    EmpresaId = ruta.EmpresaId,    // obligatorio
                    AsientoId = asiento.Id,
                    RutaId = dto.RutaId,
                    Fecha = DateTime.Now
                };

                _context.Venta.Add(venta);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.SetObjectAsJson("Carrito", carrito);

            decimal total = carrito.Count * precio;
            HttpContext.Session.SetString("Total", total.ToString());

            return Ok(new
            {
                mensaje = "Asientos agregados correctamente",
                carritoCount = carrito.Count,
                total
            });
        }
    }
}
