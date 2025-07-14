using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasswordManager.Models;
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

        private User? _loggedInUser;
        public User? LoggedInUser
        {
            get => _loggedInUser;
            set { _loggedInUser = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand => new RelayCommand(obj =>
        {
            var user = _userRepository.Login(Username, Password);
            if (user != null)
            {
                LoggedInUser = user;
                Message = $"✅ Welcome, {user.Username}!";
                // TODO: Add navigation logic here if needed
            }
            else
            {
                Message = "❌ Invalid credentials.";
            }
        });

        public ICommand RegisterCommand => new RelayCommand(obj =>
        {
            var success = _userRepository.Register(Username, Password);
            Message = success ? "✅ Registration successful!" : "❌ Username already exists.";
        });

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
