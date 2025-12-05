using Microsoft.AspNetCore.Mvc;
using Busticket.Models;  
using Busticket.Data;    
using Microsoft.AspNetCore.Http; 

namespace Busticket.Controllers
{
    public class AuthController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

       
        public AuthController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

          
            var usuario = _context.Usuarios
                                  .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.Email);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View(model);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(); 
        }

        // POST: /Auth/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existe = _context.Usuarios.Any(u => u.Email == model.Email);
            if (existe)
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado");
                return View(model);
            }

          
            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Password = model.Password 
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

           
            HttpContext.Session.SetString("Usuario", usuario.Email);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
    
}

