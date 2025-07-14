using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasswordManager.Models;
using PasswordManager.Repositories;
using System.Collections.ObjectModel;

namespace PasswordManager.GUI.ViewModels
{
    public class AdminDashboardViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;

        public AdminDashboardViewModel(UserRepository userRepo)
        {
            _userRepository = userRepo;
            LoadUsers();
        }

        public ObservableCollection<User> Users { get; } = new();

        private string _usernameToDelete = string.Empty;
        public string UsernameToDelete
        {
            get => _usernameToDelete;
            set
            {
                if (_usernameToDelete != value)
                {
                    _usernameToDelete = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand DeleteUserCommand => new RelayCommand(obj =>
        {
            if (_userRepository.DeleteUser(UsernameToDelete))
            {
                Message = "✅ User deleted.";
                LoadUsers();
            }
            else
            {
                Message = "❌ User not found.";
            }
        });

        private void LoadUsers()
        {
            Users.Clear();
            foreach (var user in _userRepository.GetAllUsers())
                Users.Add(user);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
