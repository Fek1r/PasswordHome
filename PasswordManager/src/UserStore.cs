using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class UserStore
{
    private const string FileName = "users.json";
    private List<User> users;

    public UserStore()
    {
        Load();
    }

    // Загрузка пользователей из файла
    private void Load()
    {
        if (!File.Exists(FileName) || new FileInfo(FileName).Length == 0)
        {
            users = new List<User>();
            return;
        }

        try
        {
            string json = File.ReadAllText(FileName);
            users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке пользователей: " + ex.Message);
            users = new List<User>();
        }
    }

    // Сохранение пользователей в файл
    private void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при сохранении пользователей: " + ex.Message);
        }
    }

    // Регистрация нового пользователя
    public bool Register(string username, string password)
    {
        if (users.Any(u => u.Username == username))
        {
            return false; // пользователь с таким именем уже существует
        }

        users.Add(new User { Username = username, Password = password });
        Save();
        return true;
    }

    // Авторизация пользователя
    public bool Login(string username, string password)
    {
        return users.Any(u => u.Username == username && u.Password == password);
    }
}
