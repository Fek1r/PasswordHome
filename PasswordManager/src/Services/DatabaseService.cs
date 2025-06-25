using Npgsql;

namespace PasswordManager.Services
{
    public class DatabaseService
    {
        private readonly string connectionString;

        public DatabaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void EnsureTablesExist()
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS users (
                    id SERIAL PRIMARY KEY,
                    username TEXT UNIQUE NOT NULL,
                    password_hash TEXT NOT NULL,
                    salt TEXT NOT NULL
                );";

            string createPasswordsTable = @"
                CREATE TABLE IF NOT EXISTS passwords (
                    id SERIAL PRIMARY KEY,
                    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                    resource TEXT NOT NULL,
                    password TEXT NOT NULL
                );";

            using var cmd1 = new NpgsqlCommand(createUsersTable, conn);
            cmd1.ExecuteNonQuery();

            using var cmd2 = new NpgsqlCommand(createPasswordsTable, conn);
            cmd2.ExecuteNonQuery();
        }

        public NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
