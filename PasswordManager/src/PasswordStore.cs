using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class PasswordStore
{
    private List<PasswordEntry> entries;
    private string FileName;

    public PasswordStore(string username)
    {
        FileName = $"passwords_{username}.json";
        Load();
    }

    public void Add(string resource, string password)
    {
        var existing = entries.FirstOrDefault(e => e.Resource == resource);
        if (existing != null)
            existing.Password = password;
        else
            entries.Add(new PasswordEntry { Resource = resource, Password = password });

        Save();
    }

    public string Find(string resource)
    {
        return entries.FirstOrDefault(e => e.Resource == resource)?.Password;
    }

    private void Load()
    {
        if (File.Exists(FileName))
        {
            string json = File.ReadAllText(FileName);
            entries = JsonSerializer.Deserialize<List<PasswordEntry>>(json) ?? new List<PasswordEntry>();
        }
        else
        {
            entries = new List<PasswordEntry>();
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }
}
