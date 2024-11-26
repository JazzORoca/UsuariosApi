using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AddUserAsync(User user)
        {
            const string sql = "INSERT INTO Users (Email, Name, Age) VALUES (@Email, @Name, @Age)";
            using var connection = new MySqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM Users WHERE Email = @Email";
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            const string sql = "SELECT * FROM Users";
            using var connection = new MySqlConnection(_connectionString);
            return (await connection.QueryAsync<User>(sql)).AsList();
        }

        public async Task UpdateUserAsync(User user)
        {
            const string sql = "UPDATE Users SET Name = @Name, Age = @Age WHERE Email = @Email";
            using var connection = new MySqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, user);
        }
    }
}

