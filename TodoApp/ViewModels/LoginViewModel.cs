using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Services;
using TodoApp.Views; // Potrzebne do nawigacji

namespace TodoApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return;

            var user = await _databaseService.LoginUserAsync(Username, Password);

            if (user != null)
            {
                // ZAPAMIĘTAJ KTO JEST ZALOGOWANY
                // Używamy wbudowanego w MAUI mechanizmu Preferences
                Preferences.Set("LoggedUserId", user.Id);
                Preferences.Set("LoggedUserName", user.Username);

                // Przejdź do głównego ekranu (TodoPage)
                // Używamy "///" aby wyczyścić historię nawigacji (żeby strzałka wstecz nie wracała do logowania)
                await Shell.Current.GoToAsync($"//{nameof(TodoPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd", "Błędny login lub hasło", "OK");
            }
        }

        [RelayCommand]
        async Task GoToRegister()
        {
            // Przejdź do ekranu rejestracji
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}