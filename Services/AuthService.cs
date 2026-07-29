using Npgsql;

namespace LibraryManagementSystem.Web.Services
{
    public class LoggedInUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class AuthService
    {
        private readonly DatabaseService _db;
        public LoggedInUser? CurrentUser { get; private set; }
        public bool IsAuthenticated => CurrentUser != null;

        public AuthService(DatabaseService db) => _db = db;

        public void RestoreUser(LoggedInUser? user)
        {
            CurrentUser = user;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = "SELECT id, username, full_name, role FROM users WHERE username=@u AND password_hash=@p LIMIT 1";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("u", username.Trim().ToLower());
            cmd.Parameters.AddWithValue("p", password);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                CurrentUser = new LoggedInUser
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    FullName = reader.GetString(2),
                    Role = reader.GetString(3)
                };
                return true;
            }
            return false;
        }

        public void Logout() => CurrentUser = null;
    }
}
