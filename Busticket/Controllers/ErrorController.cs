using Microsoft.AspNetCore.Mvc;

namespace Busticket.Controllers
{
    public class ErrorController : Controller
    {
        // Manejo de errores 404, 403, etc.
        [Route("Error/HttpStatus")]
        public IActionResult HttpStatus(int code)
        {
            if (code == 404)
                return View("404"); // Vista 404.cshtml

            // Puedes manejar otros códigos si quieres
            return View("Error"); // Vista genérica
        }

        // Manejo de error 500
        [Route("Error/ServerError")]
        public IActionResult ServerError()
        {
            return View("500"); // Vista 500.cshtml
        }
    }
}
