using Busticket.Data;
using Busticket.Models;
using Busticket.Services;
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
        private readonly CloudinaryService _cloudinary;

        public RutasController(ApplicationDbContext context, CloudinaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // GET: /Rutas
        public async Task<IActionResult> Index(string origen, string destino)
        {
            var rutas = _context.Rutas.AsQueryable();

            if (!string.IsNullOrEmpty(origen))
                rutas = rutas.Where(r => r.Origen.Contains(origen));

            if (!string.IsNullOrEmpty(destino))
                rutas = rutas.Where(r => r.Destino.Contains(destino));

            return View(await rutas.ToListAsync());
        }

        // GET: /Rutas/Details/5
        public async Task<IActionResult> Info(int id)
        {
            var ruta = await _context.Rutas.FirstOrDefaultAsync(r => r.RutaId == id);
            if (ruta == null) return NotFound();

            // Traer los asientos y marcar los vendidos
            var asientos = await _context.Asientos
                                         .Where(a => a.RutaId == id)
                                         .Select(a => new Asiento
                                         {
                                             AsientoId = a.AsientoId,
                                             Codigo = a.Codigo,
                                             Disponible = !_context.Ventas.Any(v => v.AsientoId == a.AsientoId)
                                         })
                                         .ToListAsync();

            var vm = new RutaDetalleViewModel
            {
                Ruta = ruta,
                Asientos = asientos
            };

            return View(vm);
        }


        // GET: /Rutas/Create
        public IActionResult Create() => View();

        // POST: /Rutas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ruta ruta, IFormFile imagen)
        {
            if (!ModelState.IsValid) return View(ruta);

            if (imagen != null && imagen.Length > 0)
                ruta.ImagenUrl = await _cloudinary.SubirImagenAsync(imagen);

            _context.Rutas.Add(ruta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Rutas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null) return NotFound();

            return View(ruta);
        }

        // POST: /Rutas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ruta ruta, IFormFile imagen)
        {
            if (id != ruta.RutaId) return BadRequest();
            if (!ModelState.IsValid) return View(ruta);

            var rutaExistente = await _context.Rutas.AsNoTracking()
                                                    .FirstOrDefaultAsync(r => r.RutaId == id);
            if (rutaExistente == null) return NotFound();

            if (imagen != null && imagen.Length > 0)
                ruta.ImagenUrl = await _cloudinary.SubirImagenAsync(imagen);
            else
                ruta.ImagenUrl = rutaExistente.ImagenUrl;

            _context.Update(ruta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Rutas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ruta = await _context.Rutas.FirstOrDefaultAsync(r => r.RutaId == id);
            if (ruta == null) return NotFound();

            return View(ruta);
        }

        // POST: /Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta != null)
            {
                _context.Rutas.Remove(ruta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Rutas/SubirImagen
        public IActionResult SubirImagen() => View();

        // POST: /Rutas/SubirImagen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirImagen(IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                TempData["Error"] = "Debes seleccionar una imagen.";
                return View();
            }

            try
            {
                var url = await _cloudinary.SubirImagenAsync(imagen);
                TempData["Success"] = "Imagen subida correctamente!";
                TempData["ImagenUrl"] = url;
                return RedirectToAction("SubirImagen");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al subir la imagen: {ex.Message}";
                return View();
            }
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
