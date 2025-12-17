using Busticket.Data;
using Busticket.Models;
using Busticket.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Busticket.Controllers
{
    [Authorize(Roles = "Empresa")]
    public class PanelEmpresaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PanelEmpresaController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 🔹 DASHBOARD EMPRESA
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (empresa == null)
                return RedirectToAction("CrearEmpresa");

            var ventas = await _context.Venta
                .Where(v => v.EmpresaId == empresa.EmpresaId)
                .Include(v => v.User)
                .Include(v => v.Empresa)
                .ToListAsync();

            var vm = new PanelEmpresaVM
            {
                Empresa = empresa,
                Ventas = ventas
            };

            return View(vm);
        }



        public IActionResult CrearEmpresa()
        {
            return View();
        }
        // 🔹 RUTAS DE LA EMPRESA
        public async Task<IActionResult> Rutas()
        {
            var userId = _userManager.GetUserId(User);

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (empresa == null)
                return Unauthorized();

            var rutas = await _context.Ruta
                .Where(r => r.EmpresaId == empresa.EmpresaId)
                .Include(r => r.CiudadOrigen)
                .Include(r => r.CiudadDestino)
                .ToListAsync();

            return View(rutas);
        }

        // 🔹 CREAR RUTA (GET)

        // 🔹 CREAR RUTA (GET)
        public IActionResult CrearRuta()
        {
            var ciudades = _context.Ciudad.ToList();

            if (!ciudades.Any())
            {
                return Content("No hay ciudades registradas. Cree ciudades primero.");
            }

            ViewBag.Ciudades = ciudades;
            return View(new Ruta());
        }

        // 🔹 CREAR RUTA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearRuta(Ruta ruta)
        {
            // 🔑 CLAVE: EmpresaId NO viene del formulario
            ModelState.Remove("EmpresaId");

            if (ruta.CiudadOrigenId == ruta.CiudadDestinoId)
            {
                ModelState.AddModelError("", "La ciudad de origen y destino no pueden ser la misma.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Ciudades = _context.Ciudad.ToList();
                return View(ruta);
            }

            var userId = _userManager.GetUserId(User);

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (empresa == null)
                return Unauthorized();

            ruta.EmpresaId = empresa.EmpresaId;

            _context.Ruta.Add(ruta);
            await _context.SaveChangesAsync();

            // 👉 Asientos (por ahora 20)
            var asientos = Enumerable.Range(1, 20)
                .Select(i => new Asiento
                {
                    Numero = i,
                    RutaId = ruta.RutaId
                }).ToList();

            _context.Asiento.AddRange(asientos);
            await _context.SaveChangesAsync();

            return RedirectToAction("Rutas");
        }

    }
}
