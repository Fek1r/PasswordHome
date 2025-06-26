using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasswordManager.Repositories;

namespace PasswordManager.GUI.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;

        public AuthViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand => new RelayCommand(_ =>
        {
            var user = _userRepository.Login(Username, Password);
            Message = user != null ? "✅ Login success!" : "❌ Login failed.";
        });

        public ICommand RegisterCommand => new RelayCommand(_ =>
        {
            var success = _userRepository.Register(Username, Password);
            Message = success ? "✅ Registered!" : "❌ Username taken.";
        });

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;

        public RelayCommand(Action<object?> execute) => _execute = execute;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;
    }
}
