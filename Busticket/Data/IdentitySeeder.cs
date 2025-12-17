using Microsoft.AspNetCore.Identity;

namespace Busticket.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 🔹 Roles que quieres en la app
            string[] roles = { "Admin", "Conductor", "Cliente" ,"Empresa" };

            // 🔥 Crear roles si no existen
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 🔥 Crear usuario administrador por defecto
            string adminEmail = "admin@busticket.com";
            string adminPassword = "Admin123@"; 

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
