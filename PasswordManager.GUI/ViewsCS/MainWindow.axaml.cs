using Avalonia.Controls;
using PasswordManager.GUI.ViewModels;
using PasswordManager.Repositories;
using PasswordManager.Services;

namespace PasswordManager.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=10021711;Database=postgres";

            var dbService = new DatabaseService(connectionString);
            dbService.EnsureTablesExist();

            var userRepository = new UserRepository(connectionString);
            DataContext = new AuthViewModel(userRepository);
        }
    }
}
