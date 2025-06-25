using PasswordManager.Models;
using Npgsql;
using System.Collections.Generic;

namespace PasswordManager.Repositories
{
    public class PasswordRepository
    {
        private readonly string connectionString;

        public PasswordRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddPassword(int userId, string resource, string password)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = @"
                INSERT INTO passwords (user_id, resource, password)
                VALUES (@user_id, @resource, @password);";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("resource", resource);
            cmd.Parameters.AddWithValue("password", password);

            cmd.ExecuteNonQuery();
        }

        public string? FindPassword(int userId, string resource)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = @"
                SELECT password
                FROM passwords
                WHERE user_id = @user_id AND resource = @resource
                LIMIT 1;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("resource", resource);

            var result = cmd.ExecuteScalar();

            return result != null ? result.ToString() : null;
        }

        public List<PasswordEntry> GetPasswordsForUser(int userId)
        {
            var passwords = new List<PasswordEntry>();

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = @"
                SELECT id, user_id, resource, password
                FROM passwords
                WHERE user_id = @user_id;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("user_id", userId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                passwords.Add(new PasswordEntry
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Resource = reader.GetString(2),
                    Password = reader.GetString(3)
                });
            }

            return passwords;
        }
    }
}
