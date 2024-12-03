using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data;
using BCrypt.Net;

namespace UsuariosApi.Services
{
    public class UserService
    {
        private readonly UserDao _userDao;

        public UserService(string connectionString)
        {
            _userDao = UserDao.GetInstance(connectionString);
        }

        /// <summary>
        /// Agregar un nuevo usuario con la contraseña hasheada.
        /// </summary>
        /// <param name="user">Objeto de usuario.</param>
        /// <param name="password">Contraseña en texto plano.</param>
        /// <returns>Tarea asincrónica.</returns>
        public async Task AddUserAsync(User user, string password)
        {
            if (user.Age <= 14)
            {
                throw new ArgumentException("La edad debe ser mayor a 14.");
            }

            if (!user.Email.Contains("@gmail.com"))
            {
                throw new ArgumentException("El email debe ser una cuenta de Gmail.");
            }

            // Hashear la contraseña antes de guardar
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Guardar usuario con hash de la contraseña
            await _userDao.AddUserAsync(user);
        }

        /// <summary>
        /// Obtener un usuario por su email.
        /// </summary>
        /// <param name="email">Email del usuario.</param>
        /// <returns>Usuario si existe, null de lo contrario.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userDao.GetUserByEmailAsync(email);
        }

        /// <summary>
        /// Verificar si la contraseña ingresada coincide con la almacenada en la base de datos.
        /// </summary>
        /// <param name="email">Email del usuario.</param>
        /// <param name="password">Contraseña ingresada en texto plano.</param>
        /// <returns>True si la contraseña es válida, false de lo contrario.</returns>
        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            return await _userDao.VerifyPasswordAsync(email, password);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userDao.UpdateUserAsync(user);
        }


        /// <summary>
        /// Obtener la lista de todos los usuarios.
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userDao.GetAllUsersAsync();
        }
    }
}

