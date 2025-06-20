using System;

class Program
{
    static PasswordStore store = new PasswordStore();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1 - Add password");
            Console.WriteLine("2 - Find password");
            Console.WriteLine("3 - Exit");
            Console.Write("Choice: ");
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

        store.Add(resource, password);
        Console.WriteLine("Password saved.");
    }

    static void FindPassword()
    {
        Console.Write("Enter resource name to search: ");
        string resource = Console.ReadLine();

        string password = store.Find(resource);

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
