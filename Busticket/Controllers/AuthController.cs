using Busticket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Busticket.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                password,
                false,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(model);
            }

            var exists = await _userManager.FindByEmailAsync(model.Email);
            if (exists != null)
            {
                ViewBag.Error = "Este correo ya está registrado";
                return View(model);
            }

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Rol por defecto
                await _userManager.AddToRoleAsync(user, "Cliente");
                return RedirectToAction("Login");
            }

            ViewBag.Error = string.Join(" | ", result.Errors.Select(e => e.Description));
            return View(model);
        }

    }
}
