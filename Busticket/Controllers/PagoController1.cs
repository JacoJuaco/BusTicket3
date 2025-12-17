using Busticket.Data;
using Busticket.Models;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;



namespace Busticket.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: /Pago/Index
        // =========================
        public IActionResult Index()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var totalString = HttpContext.Session.GetString("Total");

            var asientos = string.IsNullOrEmpty(carritoJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(carritoJson);

            var total = decimal.TryParse(totalString, out var t) ? t : 0;

            string origen = "", destino = "", empresa = "", duracion = "", fecha = "", hora = "";
            int rutaId = 0;

            if (asientos.Any() && int.TryParse(asientos[0], out int numeroAsiento))
            {
                var asiento = _context.Asiento
                    .Include(a => a.Ruta).ThenInclude(r => r.CiudadOrigen)
                    .Include(a => a.Ruta).ThenInclude(r => r.CiudadDestino)
                    .Include(a => a.Ruta).ThenInclude(r => r.Empresa)
                    .FirstOrDefault(a => a.Numero == numeroAsiento);

                if (asiento != null)
                {
                    rutaId = asiento.RutaId;
                    origen = asiento.Ruta.CiudadOrigen.Nombre;
                    destino = asiento.Ruta.CiudadDestino.Nombre;
                    empresa = asiento.Ruta.Empresa.Nombre;
                    duracion = asiento.Ruta.DuracionMin + " min";
                    fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    hora = "00:00";
                }
            }

            return View(new PagoViewModel
            {
                RutaId = rutaId,
                Asientos = asientos,
                Total = total,
                Origen = origen,
                Destino = destino,
                Empresa = empresa,
                Duracion = duracion,
                Fecha = fecha,
                Hora = hora
            });
        }

        // =========================
        // POST: /Pago/FinalizarPago
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> FinalizarPago(PagoViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (model == null || model.Asientos == null || !model.Asientos.Any())
                return RedirectToAction("Index");

            if (model.Total <= 0)
                return RedirectToAction("Index");

            var asientosInt = model.Asientos
                .Select(s => int.TryParse(s, out var n) ? n : -1)
                .Where(n => n != -1)
                .ToList();

            if (!asientosInt.Any())
                return RedirectToAction("Index");

            var asientosDb = await _context.Asiento
                .Where(a =>
                    asientosInt.Contains(a.Numero) &&
                    a.RutaId == model.RutaId &&
                    a.Disponible)
                .ToListAsync();

            if (!asientosDb.Any())
                return RedirectToAction("Index");

            var ruta = await _context.Ruta.FirstOrDefaultAsync(r => r.RutaId == model.RutaId);
            if (ruta == null)
                return RedirectToAction("Index");

            // ✅ DECLARADA FUERA
            Venta venta = null;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                venta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = model.Total,
                    RutaId = model.RutaId,
                    EmpresaId = ruta.EmpresaId,
                    UserId = userId
                };

                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();

                foreach (var asiento in asientosDb)
                    asiento.Disponible = false;

                foreach (var asiento in asientosDb)
                {
                    _context.Boleto.Add(new Boleto
                    {
                        VentaId = venta.VentaId,
                        UserId = userId,
                        RutaId = venta.RutaId,
                        AsientoId = asiento.AsientoId,
                        Precio = ruta.Precio,
                        FechaCompra = DateTime.Now,
                        Codigo = $"BT-{Guid.NewGuid():N}".Substring(0, 10).ToUpper()
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            HttpContext.Session.Remove("Carrito");
            HttpContext.Session.Remove("Total");

            // ✅ AHORA SÍ EXISTE
            return RedirectToAction(
        "ConfirmacionPago",
        "Pago",
        new { ventaId = venta.VentaId }
    );

        }


      

        //confirmacionpago//


        [Authorize]
        public async Task<IActionResult> ConfirmacionPago(int ventaId)
        {
            var venta = await _context.Venta
                .Include(v => v.Empresa)
                .Include(v => v.Ruta)
                .Include(v => v.Boletos)
                .FirstOrDefaultAsync(v => v.VentaId == ventaId);

            if (venta == null)
                return RedirectToAction("Index", "Home");

            return View(venta);
        }

        // =========================
        // GET: /Pago/DescargarBoleto
        // =========================
        public async Task<IActionResult> DescargarBoleto(int ventaId)
        {
            var boletos = await _context.Boleto
     .Include(b => b.Asiento)
     .Include(b => b.Ruta)
         .ThenInclude(r => r.CiudadOrigen)
     .Include(b => b.Ruta)
         .ThenInclude(r => r.CiudadDestino)
     .Include(b => b.Ruta)
         .ThenInclude(r => r.Empresa)
     .Where(b => b.VentaId == ventaId)
     .ToListAsync();


            if (!boletos.Any())
                return Content("No hay boletos");

            byte[] pdfBytes;

            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);

                // 👉 TAMAÑO TIPO BOLETO (vertical)
                var pageSize = new PageSize(250, 500);
                var doc = new Document(pdf, pageSize);
                doc.SetMargins(20, 20, 20, 20);

                var normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // ===============================
                // ENCABEZADO
                // ===============================
                doc.Add(new Paragraph("BUS TICKET")
                    .SetFont(bold)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.DARK_GRAY));

                doc.Add(new Paragraph("Comprobante de viaje")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER));

                doc.Add(new LineSeparator(new SolidLine()));

                foreach (var b in boletos)
                {
                    doc.Add(new Paragraph("RUTA")
                        .SetFont(bold)
                        .SetFontSize(11));
                    doc.Add(new Paragraph(
                        $"{b.Ruta.CiudadOrigen!.Nombre} → {b.Ruta.CiudadDestino!.Nombre}"
                    ).SetFontSize(10));


                    doc.Add(new Paragraph($"Asiento: {b.Asiento.Numero}")
                        .SetFontSize(10));

                    doc.Add(new Paragraph($"Código: {b.Codigo}")
                        .SetFontSize(10));

                    doc.Add(new Paragraph($"Precio: {b.Precio:N0} COP")
                        .SetFont(bold)
                        .SetFontSize(11));

                    // ===============================
                    // QR
                    // ===============================
                    var qrData = $"VENTA:{ventaId}|BOLETO:{b.Codigo}|ASIENTO:{b.Asiento.Numero}";
                    var qr = new BarcodeQRCode(qrData);
                    var qrImage = new Image(qr.CreateFormXObject(pdf))
                        .SetWidth(100)
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    doc.Add(new Paragraph(" "));
                    doc.Add(qrImage);

                    doc.Add(new Paragraph(" "));
                    doc.Add(new LineSeparator(new DashedLine()));
                }

                // ===============================
                // TOTAL
                // ===============================
                doc.Add(new Paragraph($"TOTAL PAGADO: {boletos.Sum(x => x.Precio):N0} COP")
                    .SetFont(bold)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.CENTER));

                doc.Add(new Paragraph("Gracias por viajar con nosotros")
                    .SetFontSize(9)
                    .SetTextAlignment(TextAlignment.CENTER));

                doc.Close();
                pdfBytes = ms.ToArray();
            }

            return File(pdfBytes, "application/pdf", $"Boleto_{ventaId}.pdf");
        }

    }
}
