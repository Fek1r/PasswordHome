using PasswordManager.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool Register(string username, string password)
        {
            if (UserExists(username))
                return false;

            // Создаем соль и хеш пароля
            var salt = GenerateSalt();
            var passwordHash = ComputeHash(password, salt);

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = @"
                INSERT INTO users (username, password_hash, salt)
                VALUES (@username, @password_hash, @salt);";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password_hash", passwordHash);
            cmd.Parameters.AddWithValue("salt", salt);

            cmd.ExecuteNonQuery();

            return true;
        }

        public User? Login(string username, string password)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = @"
                SELECT id, username, password_hash, salt
                FROM users
                WHERE username = @username;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("username", username);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            var storedHash = reader.GetString(2);
            var salt = reader.GetString(3);

            var computedHash = ComputeHash(password, salt);

            if (storedHash != computedHash)
                return null;

            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = storedHash,
                Salt = salt
            };
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = "SELECT id, username, password_hash, salt FROM users;";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    Salt = reader.GetString(3)
                });
            }

            return users;
        }

        public bool DeleteUser(string username)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = "DELETE FROM users WHERE username = @username;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("username", username);

            int affected = cmd.ExecuteNonQuery();
            return affected > 0;
        }

        private bool UserExists(string username)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string query = "SELECT 1 FROM users WHERE username = @username LIMIT 1;";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("username", username);

            var result = cmd.ExecuteScalar();
            return result != null;
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string ComputeHash(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }
    }
}
