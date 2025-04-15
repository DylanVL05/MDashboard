using MDashboard.Models;
using MDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDashboard.Repository
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> SaveUsuarioAsync(Usuario usuario);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task<Usuario> UpdateUsuarioByAsync(int id, Usuario updatedUsuario);
        Task<Usuario> IniciarSesionAsync(string email, string passwordHash);
        Task<bool> RegistrarUsuarioAsync(Usuario usuario);
    }
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        // Obtener todos los usuarios
        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await ReadAsync();
        }

        // Obtener usuario por ID
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            var usuarios = await ReadAsync();
            return usuarios.SingleOrDefault(u => u.Id == id);
        }

        // Eliminar usuario
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await GetUsuarioByIdAsync(id);    
            if (usuario != null)
            {
                return await DeleteAsync(usuario);
            }
            return false;
        }

        // Guardar usuario
        public async Task<Usuario> SaveUsuarioAsync(Usuario usuario)
        {
            var exists = usuario.Id > 0;
            if (exists)
            {
                await UpdateAsync(usuario);
            }
            else
            {
                await CreateAsync(usuario);
            }

            var updatedUsuarios = await ReadAsync();
            return updatedUsuarios.SingleOrDefault(u => u.Id == usuario.Id);
        }

        // Modificar usuario
        public async Task<Usuario> UpdateUsuarioByAsync(int id, Usuario updatedUsuario)
        {
            var existingUsuario = await GetUsuarioByIdAsync(id);

            existingUsuario.Id = updatedUsuario.Id;
            existingUsuario.Nombre = updatedUsuario.Nombre;
            existingUsuario.Email = updatedUsuario.Email;
            existingUsuario.PasswordHash = updatedUsuario.PasswordHash;
            existingUsuario.Rol = updatedUsuario.Rol;
            existingUsuario.FechaRegistro = updatedUsuario.FechaRegistro;

            await UpdateAsync(existingUsuario);
            return existingUsuario;
        }

        // Iniciar sesión
        public async Task<Usuario> IniciarSesionAsync(string email, string passwordHash)
        {
            var usuarios = await ReadAsync();
            return usuarios.FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
        }

        // Registrar usuario
        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            var exists = (await ReadAsync()).Any(u => u.Email == usuario.Email);
            if (exists)
            {
                throw new Exception("El usuario ya está registrado.");
            }

            usuario.FechaRegistro = DateTime.UtcNow;
            await CreateAsync(usuario);
            return true;
        }
    }
}
