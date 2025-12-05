using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/HttpStatus")]
    public IActionResult HttpStatus(int code)
    {
        if (code == 404)
            return View("~/Views/Shared/404.cshtml");

        return View("~/Views/Shared/500.cshtml");
    }

    [Route("Error/ServerError")]
    public IActionResult ServerError()
    {
        return View("~/Views/Shared/500.cshtml");
    }
}
