using Busticket.Data;
using Busticket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Busticket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmpresaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR EMPRESAS
        public async Task<IActionResult> Index()
        {
            var empresas = await _context.Empresa.ToListAsync();
            return View(empresas);
        }

        // CREAR EMPRESA GET
        public IActionResult Crear()
        {
            return View(new Empresa());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Empresa empresa)
        {
            if (!ModelState.IsValid)
                return View(empresa);

            _context.Empresa.Add(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        // EDITAR EMPRESA GET
        public async Task<IActionResult> Editar(int id)
        {
            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null) return NotFound();
            return View(empresa);
        }

        // EDITAR EMPRESA POST
        [HttpPost]
        public async Task<IActionResult> Editar(Empresa empresa)
        {
            if (!ModelState.IsValid)
                return View(empresa);

            _context.Empresa.Update(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ELIMINAR EMPRESA GET
        public async Task<IActionResult> Eliminar(int id)
        {
            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null) return NotFound();
            return View(empresa);
        }

        // ELIMINAR EMPRESA POST
        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null) return NotFound();

            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
