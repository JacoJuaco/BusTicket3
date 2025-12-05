using Busticket.Data;
using Busticket.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conexión a SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 🔹 Registro de servicios
builder.Services.AddSingleton<CloudinaryService>();

// 🔹 MVC
builder.Services.AddControllersWithViews();

// 🔹 Sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ----------------------------------------------------------------------
// 🔹 Manejo de errores personalizados (404 y 500)
// ----------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    // Error 500
    app.UseExceptionHandler("/Error/ServerError");

    // Error 404 y otros códigos
    app.UseStatusCodePagesWithReExecute("/Error/HttpStatus", "?code={0}");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔹 Habilitar sesión ANTES de Authorization
app.UseSession();

app.UseAuthorization();

// 🔹 Ruta por defecto → Auth/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();
