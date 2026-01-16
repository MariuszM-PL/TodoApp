using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Helpers;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel obsługujący proces rejestracji nowego użytkownika.
    /// Zawiera logikę walidacji danych, sprawdzania dostępności loginu oraz bezpiecznego zapisu hasła.
    /// </summary>
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        /// <summary>Nazwa użytkownika wprowadzona w formularzu rejestracji.</summary>
        [ObservableProperty]
        string username = string.Empty;

        /// <summary>Hasło wprowadzone w formularzu rejestracji (przed haszowaniem).</summary>
        [ObservableProperty]
        string password = string.Empty;

        /// <summary>
        /// Konstruktor inicjalizujący serwis bazy danych SQLite.
        /// </summary>
        /// <param name="databaseService">Serwis dostępu do danych.</param>
        public RegisterViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        #region Komendy (Commands)

        /// <summary>
        /// Wykonuje pełną procedurę rejestracji: walidację długości danych,
        /// sprawdzenie czy login jest zajęty, haszowanie hasła i zapis do bazy.
        /// </summary>
        [RelayCommand]
        async Task Register()
        {
            // 1. Walidacja: Czy pola zostały wypełnione
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wprowadź nazwę użytkownika i hasło.", "OK");
                return;
            }

            // 2. Walidacja: Wymagania dotyczące długości danych
            if (Username.Length < 3)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nazwa użytkownika musi mieć min. 3 znaki.", "OK");
                return;
            }

            if (Password.Length < 5)
            {
                await Shell.Current.DisplayAlert("Błąd", "Hasło musi mieć min. 5 znaków.", "OK");
                return;
            }

            // 3. Sprawdzenie unikalności: Czy użytkownik o takim loginie już istnieje w bazie
            var existingUser = await _databaseService.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await Shell.Current.DisplayAlert("Błąd", "Taki użytkownik już istnieje.", "OK");
                return;
            }

            // 4. Utworzenie nowego obiektu użytkownika z zahaszowanym hasłem
            var newUser = new User
            {
                Username = Username,
                // Nigdy nie zapisujemy hasła otwartym tekstem
                Password = PasswordHasher.HashPassword(Password)
            };

            // 5. Zapis do lokalnej bazy danych SQLite
            await _databaseService.RegisterUserAsync(newUser);

            await Shell.Current.DisplayAlert("Sukces", "Konto utworzone pomyślnie!", "OK");

            // Czyszczenie pól formularza po udanej operacji
            Username = string.Empty;
            Password = string.Empty;

            // Powrót do ekranu logowania (nawigacja wstecz w stosie Shell)
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Nawiguje użytkownika z powrotem do ekranu logowania.
        /// </summary>
        [RelayCommand]
        async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }

        #endregion
    }
}