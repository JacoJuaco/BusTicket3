using Busticket.Data;
using Microsoft.AspNetCore.Mvc;
using Busticket.Models; // Asegúrate de tener esto

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
}
