using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    // "partial" jest wymagane przez bibliotekę MVVM
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        // Te pola będą połączone z polami tekstowymi w oknie aplikacji
        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        public RegisterViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Ta funkcja uruchomi się po kliknięciu przycisku "Zarejestruj"
        [RelayCommand]
        async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wypełnij wszystkie pola", "OK");
                return;
            }

            // Sprawdź czy użytkownik już istnieje
            var existingUser = await _databaseService.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await Shell.Current.DisplayAlert("Błąd", "Taki użytkownik już istnieje", "OK");
                return;
            }

            // Utwórz nowego użytkownika
            var newUser = new User
            {
                Username = Username,
                Password = Password
            };

            await _databaseService.RegisterUserAsync(newUser);

            await Shell.Current.DisplayAlert("Sukces", "Konto utworzone! Możesz się zalogować.", "OK");

            // Wróć do ekranu logowania
            await Shell.Current.GoToAsync("..");
        }
    }
}