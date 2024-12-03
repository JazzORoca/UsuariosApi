namespace UserManagement.Data
{
    public class User
    {
        public int Id { get; set; }                  // Identificador único del usuario (clave primaria)
        public string Email { get; set; } = string.Empty; // Email del usuario
        public string Name { get; set; } = string.Empty;  // Nombre del usuario
        public int Age { get; set; }                // Edad del usuario (validada en la API)
        public string PasswordHash { get; set; } = string.Empty; // Hash de la contraseña
        public DateTime CreatedAt { get; set; }
    }
}
