using Busticket.Data;
using Microsoft.AspNetCore.Mvc;

namespace Busticket.Controllers
{
    public class PlanesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rutas = _context.Ruta.ToList(); 
            return View(rutas);                  
        }
    }
}
