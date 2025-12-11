using Busticket.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// Servicios
// ----------------------------------------------------

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ----------------------------------------------------
// Middleware de errores
// ----------------------------------------------------

if (app.Environment.IsDevelopment())
{
    // Solo en desarrollo
    app.UseDeveloperExceptionPage();
}
else
{
    // Solo en producción
    app.UseExceptionHandler("/Error/Error500");
    app.UseStatusCodePagesWithReExecute("/Error/HttpStatus", "?code={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ----------------------------------------------------
// Rutas
// ----------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

// ----------------------------------------------------
// Seed
// ----------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAndAdmin(scope.ServiceProvider);
}

app.Run();
