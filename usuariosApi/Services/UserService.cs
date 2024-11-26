using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data;

namespace UsuariosApi.Services
{
    public class UserService
    {
        private readonly UserDao _userDao;

        public UserService(string connectionString)
        {
            _userDao = UserDao.GetInstance(connectionString);
        }

        public async Task AddUserAsync(User user)
        {
            if (user.Age <= 14 || !user.Email.Contains("@gmail.com"))
            {
                throw new ArgumentException("Invalid user data");
            }
            await _userDao.AddUserAsync(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userDao.GetUserByEmailAsync(email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userDao.GetAllUsersAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user.Age <= 14 || !user.Email.Contains("@gmail.com"))
            {
                throw new ArgumentException("Invalid user data");
            }
            await _userDao.UpdateUserAsync(user);
        }
    }
}

