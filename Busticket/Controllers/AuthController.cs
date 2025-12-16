using Busticket.Data;
using Busticket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Busticket.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        /* ===================== LOGIN ===================== */

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Ingrese correo y contraseña";
                return View();
            }

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

            if (!result.Succeeded)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }


            return RedirectToAction("Index", "Home");
        }

        /* ===================== REGISTER ===================== */

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            /* 🔹 Validaciones base */
            if (!ModelState.IsValid)
                return View(model);

            /* 🔹 Validaciones según tipo */
            if (model.TipoUsuario == "Empresa")
            {
                if (string.IsNullOrWhiteSpace(model.NombreEmpresa))
                    ModelState.AddModelError("NombreEmpresa", "El nombre de la empresa es obligatorio");

                if (string.IsNullOrWhiteSpace(model.Nit))
                    ModelState.AddModelError("Nit", "El NIT es obligatorio");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.Nombre))
                    ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            }

            if (!ModelState.IsValid)
                return View(model);

            /* 🔹 Verificar si el correo ya existe */
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Este correo ya está registrado";
                return View(model);
            }

            /* 🔹 Crear usuario Identity */
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                ViewBag.Error = string.Join(" | ", result.Errors.Select(e => e.Description));
                return View(model);
            }

            /* 🔹 Asignar rol */
            var role = model.TipoUsuario == "Empresa"
                ? "Empresa"
                : "Cliente";

            await _userManager.AddToRoleAsync(user, role);

            /* 🔹 Crear Empresa automáticamente */
            if (model.TipoUsuario == "Empresa")
            {
                var empresa = new Empresa
                {
                    Nombre = model.NombreEmpresa!,
                    Nit = model.Nit!,
                    Email = model.Email,
                    UserId = user.Id
                };

                _context.Empresa.Add(empresa);
                await _context.SaveChangesAsync();
            }

            /* 🔹 Login automático */
            await _signInManager.SignInAsync(user, false);

            /* 🔹 Redirección */
          

            return RedirectToAction("Index", "Home");
        }

        /* ===================== LOGOUT ===================== */

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
