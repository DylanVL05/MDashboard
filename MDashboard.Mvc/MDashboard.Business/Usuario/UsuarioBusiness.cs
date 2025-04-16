using MDashboard.Models;
using MDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDashboard.Business
{
    public interface IUsuarioBusiness
    {
        // CRUD básico:
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> SaveUsuarioAsync(Usuario usuario);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<Usuario> UpdateUsuarioByAsync(int id, Usuario usuario);

        // Autenticación y registro:
        Task<Usuario> IniciarSesionAsync(string email, string passwordHash);
        Task<bool> RegistrarUsuarioAsync(Usuario usuario);
        Task<Usuario> GetUsuarioByIdAsync(int id);
    }

    public class UsuarioBusiness : IUsuarioBusiness
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioBusiness(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllUsuariosAsync();
        }

        // Guardar usuario
        public async Task<Usuario> SaveUsuarioAsync(Usuario usuario)
        {
            var savedUsuario = await _usuarioRepository.SaveUsuarioAsync(usuario);
            return savedUsuario;
        }

        // Eliminar usuario
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            return await _usuarioRepository.DeleteUsuarioAsync(id);
        }

        // Actualizar usuario
        public async Task<Usuario> UpdateUsuarioByAsync(int id, Usuario usuario)
        {
            return await _usuarioRepository.UpdateUsuarioByAsync(id, usuario);
        }

        // Iniciar sesión
        public async Task<Usuario> IniciarSesionAsync(string email, string passwordHash)
        {
            var usuario = await _usuarioRepository.IniciarSesionAsync(email, passwordHash);
            if (usuario == null)
            {
                throw new Exception("Credenciales incorrectas.");
            }

            return usuario;
        }

        // Registrar usuario
        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.RegistrarUsuarioAsync(usuario);
        }
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _usuarioRepository.GetUsuarioByIdAsync(id);
        }
    }
}
