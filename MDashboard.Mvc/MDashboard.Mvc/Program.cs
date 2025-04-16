using MDashboard.Business;
using MDashboard.Business.Factory;
using MDashboard.Business.Services;
using MDashboard.Business.Widgets;
using MDashboard.Data.Models;
using MDashboard.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la base de datos
builder.Services.AddDbContext<MediaDashboardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Inyecci�n de dependencias
builder.Services.AddScoped<WidgetApiFactory>();
builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();
builder.Services.AddScoped<IWidgetBusiness, WidgetBusiness>();
builder.Services.AddScoped<WidgetBusiness>();
builder.Services.AddScoped<WidgetService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IConfiguracionWidgetsBusiness, ConfiguracionWidgetsBusiness>();
builder.Services.AddScoped<IConfiguracionWidgetsRepository, ConfiguracionWidgetsRepository>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// Sesi�n
builder.Services.AddSession();

// AUTENTICACI�N CON COOKIES Y CLAIMS
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

// AUTENTICACI�N Y AUTORIZACI�N
app.UseAuthentication(); // <- �IMPORTANTE!
app.UseAuthorization();

// Rutas protegidas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

// Routes that require authorization
app.MapControllerRoute(
    name: "authenticated",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();