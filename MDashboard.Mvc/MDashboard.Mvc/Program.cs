using MDashboard.Business;
using MDashboard.Business.Factory;
using MDashboard.Business.Services;
using MDashboard.Data.Models;
using MDashboard.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
builder.Services.AddDbContext<MediaDashboardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Inyección de dependencias
builder.Services.AddScoped<WidgetApiFactory>();
builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();
builder.Services.AddScoped<WidgetService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// Sesión
builder.Services.AddSession();

// AUTENTICACIÓN CON COOKIES Y CLAIMS
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Usuario/AccessDenied"; // Puedes crear esta vista si deseas
    });

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// AUTENTICACIÓN Y AUTORIZACIÓN
app.UseAuthentication(); // <- ¡IMPORTANTE!
app.UseAuthorization();

// Rutas protegidas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
);

// Rutas para usuarios no autenticados
app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Usuario}/{action=Login}/{id?}"
).RequireAuthorization(); // Esto protegerá que solo usuarios logueados puedan acceder

app.Run();