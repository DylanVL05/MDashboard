using Microsoft.AspNetCore.Mvc;
using MDashboard.Business;
using MDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDashboard.Mvc.Controllers
{
    public abstract class MainUsuarioController : Controller
    {
        private readonly IUsuarioBusiness _usuarioBusiness;

        public MainUsuarioController(IUsuarioBusiness usuarioBusiness)
        {
            _usuarioBusiness = usuarioBusiness;
        }

        // Métodos CRUD para Usuario

        // Método para obtener todos los usuarios
        protected async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            return await _usuarioBusiness.GetAllUsuariosAsync();
        }

        // Método para guardar un usuario
        protected async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            return await _usuarioBusiness.SaveUsuarioAsync(usuario);
        }

        // Método para eliminar un usuario
        protected async Task<bool> DeleteUsuario(int id)
        {
            return await _usuarioBusiness.DeleteUsuarioAsync(id);
        }

        // Método para actualizar un usuario
        protected async Task<Usuario> UpdateUsuario(int id, Usuario usuario)
        {
            return await _usuarioBusiness.UpdateUsuarioByAsync(id, usuario);
        }

        // Métodos específicos para autenticación y registro

        // Método para iniciar sesión
        protected async Task<Usuario> IniciarSesion(string email, string passwordHash)
        {
            return await _usuarioBusiness.IniciarSesionAsync(email, passwordHash);
        }

        // Método para registrar un nuevo usuario
        protected async Task<bool> RegistrarUsuario(Usuario usuario)
        {
            return await _usuarioBusiness.RegistrarUsuarioAsync(usuario);
        }
    }
}
