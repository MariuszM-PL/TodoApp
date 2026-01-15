using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Services;
using TodoApp.Views;
using TodoApp.Helpers; // Dodane, aby widzieć PasswordHasher

namespace TodoApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        string username = string.Empty;

        [ObservableProperty]
        string password = string.Empty;

        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        async Task Login()
        {
            // 1. Walidacja: Czy pola są wypełnione?
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wprowadź login i hasło", "OK");
                return;
            }

            // 2. Pobieramy użytkownika z bazy po samym loginie (używamy Twojej metody z DatabaseService)
            var user = await _databaseService.GetUserByUsernameAsync(Username);

            // 3. Haszujemy hasło wpisane w formularzu
            string inputHashedPassword = PasswordHasher.HashPassword(Password);

            // 4. Sprawdzamy:
            // - Czy użytkownik istnieje? (user != null)
            // - Czy hasz hasła z bazy zgadza się z haszem wpisanym?
            if (user != null && user.Password == inputHashedPassword)
            {
                // ZALOGOWANO POMYŚLNIE

                // Zapisujemy sesję
                Preferences.Set("LoggedUserId", user.Id);
                Preferences.Set("LoggedUserName", user.Username);

                // Czyścimy pola hasła w formularzu (dla bezpieczeństwa)
                Password = string.Empty;

                // Przejdź do głównego ekranu
                await Shell.Current.GoToAsync($"//{nameof(TodoPage)}");
            }
            else
            {
                // Błąd logowania
                await Shell.Current.DisplayAlert("Błąd", "Błędny login lub hasło", "OK");
            }
        }

        [RelayCommand]
        async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}