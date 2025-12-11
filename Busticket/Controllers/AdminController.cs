using Busticket.Data;
using Busticket.Models;
using Busticket.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Busticket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // PANEL ADMIN

        public async Task<IActionResult> Index()
    {
        var reporte = await _context.Venta
            .Include(v => v.User)        // propiedad correcta
            .Include(v => v.Empresa)
            .Include(v => v.Ruta)        // opcional
            .ToListAsync();

        var vm = reporte.Select(v => new ReporteVentaVM1
        {
            VentaId = v.VentaId,
            Usuario = v.User.UserName,
            Empresa = v.Empresa.Nombre,
            Fecha = v.Fecha

        }).ToList();

        return View(vm);
    }


    // LISTAR RUTAS
    public async Task<IActionResult> Rutas()
        {
            var rutas = await _context.Ruta
                .Include(r => r.Empresa)
                .Include(r => r.CiudadOrigen)
                .Include(r => r.CiudadDestino)
                .ToListAsync();

            return View(rutas);
        }

        // MÉTODO AUXILIAR PARA SELECTS
        private void CargarViewBags()
        {
            ViewBag.Ciudades = _context.Ciudad.ToList();
            ViewBag.Empresas = _context.Empresa.ToList();
        }

        // CREAR RUTA (GET)
        public IActionResult CrearRuta()
        {
            CargarViewBags();
            return View(new Ruta());
        }

        // CREAR RUTA (POST)
        [HttpPost]
        public IActionResult CrearRuta(Ruta ruta)
        {
            if (!ModelState.IsValid)
            {
                CargarViewBags();

                var errores = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                TempData["ErrorMessage"] = string.Join(", ", errores);

                return View(ruta);
            }

            _context.Ruta.Add(ruta);
            _context.SaveChanges();

            // Crear 20 asientos automáticamente
            var asientos = Enumerable.Range(1, 20)
                                     .Select(i => new Asiento { Numero = i, RutaId = ruta.RutaId })
                                     .ToList();

            _context.Asiento.AddRange(asientos);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Ruta creada correctamente con sus asientos.";
            return RedirectToAction("Rutas");
        }

        // EDITAR RUTA (GET)
        public async Task<IActionResult> EditarRuta(int id)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) return NotFound();

            CargarViewBags();
            return View(ruta);
        }

        // EDITAR RUTA (POST)
        [HttpPost]
        public async Task<IActionResult> EditarRuta(Ruta ruta)
        {
            if (!ModelState.IsValid)
            {
                CargarViewBags();

                var errores = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                TempData["ErrorMessage"] = string.Join(", ", errores);

                return View(ruta);
            }

            _context.Ruta.Update(ruta);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ruta actualizada correctamente.";
            return RedirectToAction("Rutas");
        }

        // ELIMINAR RUTA (GET)
        public async Task<IActionResult> EliminarRuta(int id)
        {
            var ruta = await _context.Ruta
                .Include(r => r.Empresa)
                .Include(r => r.CiudadOrigen)
                .Include(r => r.CiudadDestino)
                .FirstOrDefaultAsync(r => r.RutaId == id);

            if (ruta == null) return NotFound();

            return View(ruta);
        }

        // ELIMINAR RUTA (POST)
        [HttpPost, ActionName("EliminarRuta")]
        public async Task<IActionResult> EliminarRutaConfirmado(int id)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) return NotFound();

            _context.Ruta.Remove(ruta);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ruta eliminada correctamente.";
            return RedirectToAction("Rutas");
        }
    }
}
