using System;

class Program
{
    static UserStore userStore = new UserStore();
    static PasswordStore passwordStore;

    static void Main()
    {
        Console.Clear();
        Console.WriteLine("=== Password Manager ===");

        string currentUser = null;

        while (currentUser == null)
        {
            Console.WriteLine("\n1 - Register ➡️");
            Console.WriteLine("2 - Login ➡️");
            Console.Write("\nChoice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    currentUser = Register();
                    break;
                case "2":
                    currentUser = Login();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        if (currentUser == "Admin")
        {
            AdminPanel();
        }
        else
        {
            passwordStore = new PasswordStore(currentUser);
            RunPasswordManager();
        }
    }

    static string Register()
    {
        Console.Clear();
        Console.WriteLine("=== Registration ===");
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        bool success = userStore.Register(username, password);
        if (success)
        {
            Console.WriteLine("Registration successful!");
            Pause();
            return username;
        }
        else
        {
            Console.WriteLine("Username already exists.");
            Pause();
            return null;
        }
    }

    static string Login()
    {
        Console.Clear();
        Console.WriteLine("=== Login ===");
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (username == "Admin" && password == "123456789")
        {
            Console.WriteLine("Admin login successful!");
            Pause();
            return "Admin";
        }

        bool success = userStore.Login(username, password);
        if (success)
        {
            Console.WriteLine("Login successful!");
            Pause();
            return username;
        }
        else
        {
            Console.WriteLine("Invalid credentials.");
            Pause();
            return null;
        }
    }

    static void RunPasswordManager()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Password Manager ===");

            Console.WriteLine("\n1 - Add password 📥");
            Console.WriteLine("2 - Find password 🔎");
            Console.WriteLine("3 - Exit ➡️");
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
                    return;
                default:
                    Console.WriteLine("Invalid input");
                    Pause();
                    break;
            }
        }
    }

    static void AddPassword()
    {
        Console.Write("Enter resource name: ");
        string resource = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        passwordStore.Add(resource, password);
        Console.WriteLine("Password saved.");
        Pause();
    }

    static void FindPassword()
    {
        Console.Write("Enter resource name to search: ");
        string resource = Console.ReadLine();

        string password = passwordStore.Find(resource);

        if (password != null)
        {
            Console.WriteLine($"Password for {resource}: {password}");
        }
        else
        {
            Console.WriteLine("Resource not found.");
        }
        Pause();
    }

    static void AdminPanel()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Admin Panel ===");
            Console.WriteLine("1 - List all users 👥");
            Console.WriteLine("2 - Delete a user ❌");
            Console.WriteLine("3 - Exit to main menu 🔙");
            Console.Write("\nChoice: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    var users = userStore.GetAllUsers();
                    Console.WriteLine("\nRegistered users:");
                    foreach (var u in users)
                    {
                        Console.WriteLine($"- {u.Username}");
                    }
                    Pause();
                    break;

                case "2":
                    Console.Write("Enter username to delete: ");
                    string userToDelete = Console.ReadLine();
                    if (userToDelete == "Admin")
                    {
                        Console.WriteLine("Cannot delete Admin.");
                    }
                    else
                    {
                        bool deleted = userStore.DeleteUser(userToDelete);
                        Console.WriteLine(deleted ? "User deleted." : "User not found.");
                    }
                    Pause();
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Invalid input");
                    Pause();
                    break;
            }
        }
    }

    static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
