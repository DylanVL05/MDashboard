using Microsoft.AspNetCore.Mvc;
using MDashboard.Business;
using MDashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDashboard.Mvc.Controllers
{
    public class UsuarioController : MainUsuarioController
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioBusiness usuarioBusiness)
            : base(usuarioBusiness)
        {
            _logger = logger;
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
            return View("Usuarios", usuarios);
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
            return View();
        }

        // Método para iniciar sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string passwordHash)
        {
            try
            {
                var usuario = await IniciarSesion(email, passwordHash);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
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
                    await RegistrarUsuario(usuario);
                    return RedirectToAction(nameof(UserList));
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "El usuario ya está registrado.");
                }
            }
            return View(usuario);
        }
    }
}
