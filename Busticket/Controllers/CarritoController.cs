using Busticket.Data;
using Busticket.Extensions;
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

        public CarritoController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // DTO CORREGIDO
        public class AgregarCarritoDto
        {
            public int RutaId { get; set; }
            public List<int> Asientos { get; set; }
        }

        // POST: /Carrito/Agregar
        [HttpPost("Agregar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar([FromBody] AgregarCarritoDto dto)
        {
            if (dto == null || dto.Asientos == null || !dto.Asientos.Any())
            {
                return BadRequest(new
                {
                    mensaje = "No hay asientos seleccionados."
                });
            }

            var ruta = await _context.Ruta
                .FirstOrDefaultAsync(r => r.RutaId == dto.RutaId);

            if (ruta == null)
            {
                return BadRequest(new
                {
                    mensaje = "Ruta no encontrada."
                });
            }

            // 🛒 Obtener carrito desde sesión
            var carrito = HttpContext.Session
                .GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();

            foreach (var asientoId in dto.Asientos)
            {
                if (!carrito.Contains(asientoId))
                {
                    carrito.Add(asientoId);
                }
            }

            var total = carrito.Count * ruta.Precio;

            HttpContext.Session.SetObjectAsJson("Carrito", carrito);
            HttpContext.Session.SetString("Total", total.ToString());

            return Ok(new
            {
                mensaje = "Asientos agregados al carrito",
                asientos = carrito,
                total
            });
        }
    }
}
