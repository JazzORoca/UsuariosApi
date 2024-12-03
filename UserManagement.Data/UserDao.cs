using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net; // Importar la librería correctamente

namespace UserManagement.Data
{
    public class UserDao
    {
        private static UserDao? _instance;
        private readonly string _connectionString;

        // Constructor privado para implementar Singleton
        private UserDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la instancia única
        public static UserDao GetInstance(string connectionString)
        {
            return _instance ??= new UserDao(connectionString);
        }

        // Método para agregar un usuario a la base de datos
        public async Task AddUserAsync(User user)
        {
            const string sql = "INSERT INTO Users (Email, Name, Age, PasswordHash) VALUES (@Email, @Name, @Age, @PasswordHash)";
            using var connection = new MySqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, user);
        }

        // Método para verificar la contraseña de un usuario
        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            const string sql = "SELECT PasswordHash FROM Users WHERE Email = @Email";
            using var connection = new MySqlConnection(_connectionString);
            var passwordHash = await connection.QueryFirstOrDefaultAsync<string>(sql, new { Email = email });

            // Verificar si la contraseña ingresada coincide con el hash almacenado
            return passwordHash != null && BCrypt.Net.BCrypt.Verify(password, passwordHash);

        }



        // Método para obtener un usuario por su email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM Users WHERE Email = @Email";
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        // Método para obtener todos los usuarios
        public async Task<List<User>> GetAllUsersAsync()
        {
            const string sql = "SELECT * FROM Users";
            using var connection = new MySqlConnection(_connectionString);
            return (await connection.QueryAsync<User>(sql)).AsList();
        }

        // Método para actualizar los datos de un usuario
        public async Task UpdateUserAsync(User user)
        {
            const string sql = "UPDATE Users SET Name = @Name, Age = @Age WHERE Email = @Email";
            using var connection = new MySqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, user);
        }
    }
}
