using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Services;
using TodoApp.Views;
using TodoApp.Helpers;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel obsługujący proces logowania użytkownika. 
    /// Odpowiada za weryfikację poświadczeń, haszowanie haseł oraz zarządzanie sesją aplikacji.
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        /// <summary>Nazwa użytkownika wprowadzona w polu logowania.</summary>
        [ObservableProperty]
        string username = string.Empty;

        /// <summary>Hasło wprowadzone w polu logowania (przechowywane jako czysty tekst przed haszowaniem).</summary>
        [ObservableProperty]
        string password = string.Empty;

        /// <summary>
        /// Konstruktor inicjalizujący serwis bazy danych.
        /// </summary>
        /// <param name="databaseService">Serwis dostępu do danych SQLite.</param>
        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        #region Komendy (Commands)

        /// <summary>
        /// Wykonuje proces logowania: waliduje pola, sprawdza istnienie użytkownika 
        /// oraz porównuje hasze haseł.
        /// </summary>
        [RelayCommand]
        async Task Login()
        {
            // 1. Walidacja danych wejściowych
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wprowadź login i hasło", "OK");
                return;
            }

            // 2. Pobranie danych użytkownika z bazy na podstawie loginu
            var user = await _databaseService.GetUserByUsernameAsync(Username);

            // 3. Konwersja wpisanego hasła na format zahaszowany do porównania
            string inputHashedPassword = PasswordHasher.HashPassword(Password);

            // 4. Weryfikacja poświadczeń
            if (user != null && user.Password == inputHashedPassword)
            {
                // Sukces: Zapisanie danych sesji w pamięci trwałej urządzenia
                Preferences.Set("LoggedUserId", user.Id);
                Preferences.Set("LoggedUserName", user.Username);

                // Czyszczenie pola hasła ze względów bezpieczeństwa
                Password = string.Empty;

                // Przejście do głównego pulpitu zadań (wykorzystując nawigację Shell)
                await Shell.Current.GoToAsync($"//{nameof(TodoPage)}");
            }
            else
            {
                // Niepowodzenie: Wyświetlenie komunikatu o błędzie
                await Shell.Current.DisplayAlert("Błąd", "Błędny login lub hasło", "OK");
            }
        }

        /// <summary>
        /// Przekierowuje użytkownika do ekranu rejestracji nowego konta.
        /// </summary>
        [RelayCommand]
        async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        #endregion
    }
}