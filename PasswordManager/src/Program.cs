using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Services;
using System;

namespace PasswordManager
{
    class Program
    {
        private static string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=10021711;Database=postgres";

        private static DatabaseService dbService;
        private static UserRepository userRepository;
        private static PasswordRepository passwordRepository;

        private static User currentUser;

        static void Main()
        {
            Console.Clear();
            dbService = new DatabaseService(connectionString);
            dbService.EnsureTablesExist();

            userRepository = new UserRepository(connectionString);
            passwordRepository = new PasswordRepository(connectionString);

            Console.WriteLine("=== Password Manager ===");

            while (currentUser == null)
            {
                Console.WriteLine("\n1 - Register");
                Console.WriteLine("2 - Login");
                Console.Write("\nChoice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

            if (currentUser.Username == "Admin")
            {
                RunAdminMenu();
            }
            else
            {
                RunUserMenu();
            }
        }

        static void Register()
        {
            Console.Clear();
            Console.WriteLine("=== Register ===");
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var success = userRepository.Register(username, password);
            Console.WriteLine(success ? "✅ Registered successfully!" : "❌ Username already exists.");
        }

        static void Login()
        {
            Console.Clear();
            Console.WriteLine("=== Login ===");
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            currentUser = userRepository.Login(username, password);
            Console.WriteLine(currentUser != null ? "✅ Login successful!" : "❌ Invalid credentials.");
        }

        static void RunUserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Welcome, {currentUser.Username} ===");

                Console.WriteLine("\n1 - Add Password");
                Console.WriteLine("2 - Find Password");
                Console.WriteLine("3 - Logout");

                Console.Write("\nChoice: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddPassword();
                        break;
                    case "2":
                        FindPassword();
                        break;
                    case "3":
                        currentUser = null;
                        Main();
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        static void AddPassword()
        {
            Console.Write("Enter resource name: ");
            var resource = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            passwordRepository.AddPassword(currentUser.Id, resource, password);
            Console.WriteLine("🔒 Password saved.");
        }

        static void FindPassword()
        {
            Console.Write("Enter resource name: ");
            var resource = Console.ReadLine();

            var result = passwordRepository.FindPassword(currentUser.Id, resource);
            Console.WriteLine(result != null
                ? $"🔐 Password for '{resource}': {result}"
                : "❌ Resource not found.");
        }

        static void RunAdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Admin Panel ===");

                Console.WriteLine("\n1 - List All Users");
                Console.WriteLine("2 - Delete User");
                Console.WriteLine("3 - Logout");

                Console.Write("\nChoice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var users = userRepository.GetAllUsers();
                        Console.WriteLine("\nUsers:");
                        foreach (var user in users)
                        {
                            Console.WriteLine($"- {user.Username} (ID: {user.Id})");
                        }
                        break;
                    case "2":
                        Console.Write("Enter username to delete: ");
                        var uname = Console.ReadLine();
                        bool deleted = userRepository.DeleteUser(uname);
                        Console.WriteLine(deleted ? "✅ User deleted." : "❌ User not found.");
                        break;
                    case "3":
                        currentUser = null;
                        Main();
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
