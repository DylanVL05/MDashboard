using MDashboard.Business;
using MDashboard.Business.Factory;
using MDashboard.Business.Services;
using MDashboard.Data.Models;
using MDashboard.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MediaDashboardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(); // Para inyectar HttpClient
builder.Services.AddScoped<WidgetApiFactory>(); // Para la creaci�n de clientes de API
builder.Services.AddScoped<IWidgetRepository, WidgetRepository>(); // Repositorio
builder.Services.AddScoped<WidgetService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); // Repositorio de usuario 
builder.Services.AddScoped<IUsuarioBusiness, UsuarioBusiness>(); // Repositorio de usuario 
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IConfiguracionWidgetsBusiness, ConfiguracionWidgetsBusiness>();
builder.Services.AddScoped<IConfiguracionWidgetsRepository, ConfiguracionWidgetsRepository>();

//Soporte para los controladores y vistas.
builder.Services.AddControllersWithViews();

//Se agrega la sesi�n (basicamente sirve para la gestion de las sesiones)
builder.Services.AddSession(/*Aqui se puede realizar las configuraciones de la sesi�n (Investigar)*/);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
