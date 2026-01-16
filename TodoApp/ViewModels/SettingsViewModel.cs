using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Services;
using TodoApp.Helpers;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel odpowiedzialny za zarządzanie ustawieniami użytkownika,
    /// takimi jak zmiana motywu graficznego, zmiana hasła oraz wyświetlanie informacji o wersji.
    /// </summary>
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        /// <summary>Przechowuje nowe hasło wprowadzone przez użytkownika w formularzu.</summary>
        [ObservableProperty]
        string newPassword = string.Empty;

        /// <summary>Informacja o aktualnej wersji aplikacji.</summary>
        [ObservableProperty]
        string appVersion = "1.0.0";

        #region Właściwości Motywu (UI)

        /// <summary>Wskazuje, czy wybrany jest motyw zgodny z ustawieniami systemu.</summary>
        [ObservableProperty]
        bool isSystemTheme;

        /// <summary>Wskazuje, czy wybrany jest motyw jasny.</summary>
        [ObservableProperty]
        bool isLightTheme;

        /// <summary>Wskazuje, czy wybrany jest motyw ciemny.</summary>
        [ObservableProperty]
        bool isDarkTheme;

        #endregion

        /// <summary>
        /// Konstruktor inicjalizujący serwis bazy danych oraz ustawiający stan kontrolek motywu 
        /// na podstawie zapisanych preferencji użytkownika.
        /// </summary>
        /// <param name="databaseService">Serwis obsługujący operacje na bazie danych SQLite.</param>
        public SettingsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Pobranie zapisanego motywu z pamięci urządzenia (domyślnie "System")
            var currentTheme = Preferences.Get("AppTheme", "System");
            IsSystemTheme = currentTheme == "System";
            IsLightTheme = currentTheme == "Light";
            IsDarkTheme = currentTheme == "Dark";
        }

        /// <summary>
        /// Zmienia motyw graficzny aplikacji i zapisuje wybór w preferencjach systemowych.
        /// </summary>
        /// <param name="themeName">Nazwa motywu ("System", "Light" lub "Dark").</param>
        [RelayCommand]
        void ChangeTheme(string themeName)
        {
            Preferences.Set("AppTheme", themeName);

            // Wywołanie globalnej metody aplikującej styl (zdefiniowanej w App.xaml.cs)
            App.ApplyTheme();
        }

        /// <summary>
        /// Obsługuje proces zmiany hasła użytkownika, w tym walidację i haszowanie.
        /// </summary>
        [RelayCommand]
        async Task ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wpisz nowe hasło", "OK");
                return;
            }

            // Pobranie danych zalogowanego użytkownika z sesji (Preferences)
            int userId = Preferences.Get("LoggedUserId", -1);
            string username = Preferences.Get("LoggedUserName", "");

            if (userId != -1)
            {
                var userToUpdate = new Models.User
                {
                    Id = userId,
                    Username = username,
                    // Przed zapisem hasło jest bezpiecznie haszowane
                    Password = PasswordHasher.HashPassword(NewPassword)
                };

                // Aktualizacja danych w bazie SQLite
                await _databaseService.UpdateUserAsync(userToUpdate);

                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zmienione", "OK");
                NewPassword = string.Empty;
            }
        }

        /// <summary>
        /// Nawiguje użytkownika z powrotem do poprzedniej strony (głównej listy zadań).
        /// </summary>
        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}