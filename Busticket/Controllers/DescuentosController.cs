using Busticket.Data;
using Busticket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class DescuentosController : Controller
{
    private readonly ApplicationDbContext _context;

    public DescuentosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var ofertas = _context.Oferta.ToList();
        return View(ofertas);
    }

    
    public IActionResult Aplicar(int ofertaId)
    {
        return View("Aplicar");
    }
}
