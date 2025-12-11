using Busticket.Data;
using Busticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Busticket.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Busticket.Controllers
{
    public class RutasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RutasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Rutas
        public async Task<IActionResult> Index(string origen, string destino)
        {
            var rutas = _context.Ruta
                .Include(r => r.CiudadOrigen)
                .Include(r => r.CiudadDestino)
                .Include(r => r.Empresa)
                .AsQueryable();

            if (!string.IsNullOrEmpty(origen))
                rutas = rutas.Where(r => r.CiudadOrigen.Nombre.Contains(origen));

            if (!string.IsNullOrEmpty(destino))
                rutas = rutas.Where(r => r.CiudadDestino.Nombre.Contains(destino));

            return View(await rutas.ToListAsync());
        }

        // GET: /Rutas/Info/5
        public async Task<IActionResult> Info(int id)
        {
            var ruta = await _context.Ruta
                .Include(r => r.CiudadOrigen)
                .Include(r => r.CiudadDestino)
                .Include(r => r.Empresa)
                .FirstOrDefaultAsync(r => r.RutaId == id);

            if (ruta == null) return NotFound();

            // Traer los asientos y marcar los vendidos
            var asientos = await _context.Asiento
                .Where(a => a.RutaId == id)
                .Select(a => new Asiento
                {
                    AsientoId = a.AsientoId,
                    Numero = a.Numero,
                    Disponible = !_context.Venta.Any(v => v.AsientoId == a.AsientoId)
                })
                .ToListAsync();

            var vm = new RutaDetalleViewModel
            {
                Ruta = ruta,
                Asientos = asientos
            };

            return View(vm);
        }

        

        // GET: /Rutas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) return NotFound();

            ViewBag.Ciudades = _context.Ciudad.ToList();
            ViewBag.Empresas = _context.Empresa.ToList();

            return View(ruta);
        }

        // POST: /Rutas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ruta ruta, IFormFile imagen)
        {
            if (id != ruta.RutaId) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Ciudades = _context.Ciudad.ToList();
                ViewBag.Empresas = _context.Empresa.ToList();
                return View(ruta);
            }

            var rutaExistente = await _context.Ruta.AsNoTracking()
                .FirstOrDefaultAsync(r => r.RutaId == id);
            if (rutaExistente == null) return NotFound();

            /*
            if (imagen != null && imagen.Length > 0)
                ruta.ImagenUrl = await _cloudinary.SubirImagenAsync(imagen);
            else
                ruta.ImagenUrl = rutaExistente.ImagenUrl;
            */

            _context.Update(ruta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Rutas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ruta = await _context.Ruta
                .Include(r => r.CiudadOrigen)
                .Include(r => r.CiudadDestino)
                .FirstOrDefaultAsync(r => r.RutaId == id);

            if (ruta == null) return NotFound();

            return View(ruta);
        }

        // POST: /Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta != null)
            {
                _context.Ruta.Remove(ruta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Rutas/SeleccionarAsiento
        [HttpPost]
        public IActionResult SeleccionarAsiento(string asientoCodigo, int precio)
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<string>>("Carrito") ?? new List<string>();

            if (carrito.Contains(asientoCodigo))
                carrito.Remove(asientoCodigo);
            else
                carrito.Add(asientoCodigo);

            HttpContext.Session.SetObjectAsJson("Carrito", carrito);
            HttpContext.Session.SetInt32("Total", carrito.Count * precio);

            return Json(new { success = true, carritoCount = carrito.Count });
        }
    }
}
