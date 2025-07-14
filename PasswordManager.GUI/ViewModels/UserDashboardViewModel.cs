using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasswordManager.Models;
using PasswordManager.Repositories;

namespace PasswordManager.GUI.ViewModels
{
    public class UserDashboardViewModel : INotifyPropertyChanged
    {
        private readonly PasswordRepository _passwordRepository;
        private readonly User _user;

        public UserDashboardViewModel(User user, PasswordRepository passwordRepo)
        {
            _user = user;
            _passwordRepository = passwordRepo;
        }

        private string _resource = string.Empty;
        private string _password = string.Empty;
        private string _result = string.Empty;

        public string Resource
        {
            get => _resource;
            set { _resource = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string Result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(); }
        }

        public ICommand AddPasswordCommand => new RelayCommand(obj =>
        {
            _passwordRepository.AddPassword(_user.Id, Resource, Password);
            Result = "🔒 Password saved!";
        });

        public ICommand FindPasswordCommand => new RelayCommand(obj =>
        {
            var found = _passwordRepository.FindPassword(_user.Id, Resource);
            Result = found != null ? $"🔐 {Resource}: {found}" : "❌ Not found.";
        });

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
