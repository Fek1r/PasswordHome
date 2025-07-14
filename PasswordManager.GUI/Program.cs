using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace PasswordManager.GUI
{
    internal static class Program
    {
        // Entry point of the application
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration (required for initialization)
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI(); // Optional: if you're using ReactiveUI
        }
    }
}
