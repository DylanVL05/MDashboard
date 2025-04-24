using Microsoft.AspNetCore.Mvc;
using MDashboard.Business;
using MDashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MDashboard.Mvc.Controllers
{
    public class UsuarioController : MainUsuarioController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioBusiness _usuarioBusiness;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioBusiness usuarioBusiness)
            : base(usuarioBusiness)
        {
            _logger = logger;
            _usuarioBusiness = usuarioBusiness;
        }

        // Vista con la lista de usuarios
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var usuarios = await GetAllUsuarios();
            return View(usuarios);
        }

        // Filtrar usuarios por nombre o rol
        [HttpGet]
        public async Task<IActionResult> FilterUsuarios(string rol)
        {
            var usuarios = (await GetAllUsuarios()).Where(u => u.Rol == rol).ToList();
            return View("UserList", usuarios);
        }

        // Eliminar usuario
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await DeleteUsuario(id);
            return RedirectToAction(nameof(UserList));
        }

        // Vista de agregar usuario
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        // Método para agregar un usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await SaveUsuario(usuario);
                return RedirectToAction(nameof(UserList));
            }
            return View(usuario);
        }

        // Vista de editar usuario
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var usuarios = await GetAllUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            return View(usuario);
        }

        // Método para editar usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await UpdateUsuario(id, usuario);
                return RedirectToAction(nameof(UserList));
            }
            return View(usuario);
        }

        // Vista para iniciar sesión
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string passwordHash)
        {
            try
            {
                var usuario = await IniciarSesion(email, passwordHash);

                if (usuario != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
            };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("Cookies", principal);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.MensajePantalla = "Correo o Contraseña incorrecta.";
                    return View();
                }
            }
            catch
            {
                ViewBag.MensajePantalla = "Ocurrió un error inesperado. Por favor, inténtalo nuevamente.";
                return View();
            }
        }




        // Vista para registrar usuario
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Método para registrar usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Rol = "User";
                    usuario.FechaRegistro = DateTime.Now;

                    await RegistrarUsuario(usuario);
                    return RedirectToAction(nameof(Login));
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "El usuario ya está registrado.");
                }
            }
            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarSession()
        {
            await HttpContext.SignOutAsync("Cookies"); 
            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var usuario = await _usuarioBusiness.GetUsuarioByIdAsync(userId);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfil(Usuario usuario)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId != usuario.Id)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                await _usuarioBusiness.UpdateUsuarioByAsync(userId, usuario);

                var claims = new List<Claim>
 {
     new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
     new Claim(ClaimTypes.Name, usuario.Nombre),
     new Claim(ClaimTypes.Email, usuario.Email),
     new Claim(ClaimTypes.Role, usuario.Rol)
 };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                TempData["MensajePantalla"] = "Perfil actualizado correctamente";
                return RedirectToAction("MiPerfil");
            }

            return View("MiPerfil", usuario);
        }

    }
}
