using Avalonia.Controls;
using PasswordManager.Services;
using PasswordManager.Repositories;
using PasswordManager.GUI.ViewModels;

namespace PasswordManager.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // подключаем репозиторий как в консоли
            string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=10021711;Database=postgres";
            var dbService = new DatabaseService(connectionString);
            dbService.EnsureTablesExist();

            var userRepo = new UserRepository(connectionString);
            this.DataContext = new AuthViewModel(userRepo);
        }
    }
}
