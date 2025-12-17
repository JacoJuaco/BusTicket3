using Busticket.Data;
using Busticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Busticket.Controllers
{
    public class DescuentosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static Random rnd = new Random();

        public DescuentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // LISTAR OFERTAS
        // =========================
        public IActionResult Index()
        {
            var ofertas = _context.Oferta
                .Include(o => o.Empresa)
                .Where(o => o.Vigente)
                .ToList();

            return View(ofertas);
        }

        // =========================
        // GENERAR OFERTAS RANDOM
        // =========================
        public IActionResult GenerarOfertas()
        {
            var empresas = _context.Empresa.ToList();

            if (!empresas.Any())
                return RedirectToAction("Index");

            string[] titulos =
            {
                "Viaja más barato",
                "Promo especial",
                "Descuento exclusivo",
                "Oferta por tiempo limitado",
                "Ahorra en tu viaje"
            };

            foreach (var empresa in empresas)
            {
                // ❌ evitar duplicados
                bool yaExiste = _context.Oferta
                    .Any(o => o.EmpresaId == empresa.EmpresaId && o.Vigente);

                if (yaExiste)
                    continue;

                int descuento = rnd.Next(10, 41); // 10% - 40%

                _context.Oferta.Add(new Oferta
                {
                    EmpresaId = empresa.EmpresaId,
                    Titulo = titulos[rnd.Next(titulos.Length)],
                    Descripcion = $"Obtén {descuento}% de descuento viajando con {empresa.Nombre}",
                    Descuento = descuento,
                    Vigente = true
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // APLICAR DESCUENTO
        // =========================
        public IActionResult Aplicar(int ofertaId)
        {
            var oferta = _context.Oferta
                .FirstOrDefault(o => o.OfertaId == ofertaId && o.Vigente);

            if (oferta == null)
                return RedirectToAction("Index");

            HttpContext.Session.SetInt32("Descuento", oferta.Descuento);
            HttpContext.Session.SetInt32("EmpresaOferta", oferta.EmpresaId);

            return RedirectToAction("Index", "Pago");
        }
    }
}
