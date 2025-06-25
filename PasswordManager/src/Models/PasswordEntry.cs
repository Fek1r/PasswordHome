namespace PasswordManager.Models
{
    public class PasswordEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Resource { get; set; }
        public string Password { get; set; }
    }
}
