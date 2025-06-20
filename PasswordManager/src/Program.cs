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

        passwordStore = new PasswordStore(currentUser);
        RunPasswordManager();
    }

    static string Register()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        bool success = userStore.Register(username, password);
        if (success)
        {
            Console.WriteLine("Registration successful!");
            return username;
        }
        else
        {
            Console.WriteLine("Username already exists.");
            return null;
        }
    }

    static string Login()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        bool success = userStore.Login(username, password);
        if (success)
        {
            Console.WriteLine("Login successful!");
            return username;
        }
        else
        {
            Console.WriteLine("Invalid credentials.");
            return null;
        }
    }

    static void RunPasswordManager()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== Password Manager ===");

            string currentUser = null;

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
    }
}
