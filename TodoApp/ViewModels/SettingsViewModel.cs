using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Services;
using TodoApp.Helpers; // Potrzebne do haszowania hasła

namespace TodoApp.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        string newPassword;

        [ObservableProperty]
        string appVersion = "1.0.0";

        // --- POLA DO OBSŁUGI MOTYWU ---
        [ObservableProperty]
        bool isSystemTheme;

        [ObservableProperty]
        bool isLightTheme;

        [ObservableProperty]
        bool isDarkTheme;

        public SettingsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Ustawiamy kółeczka motywu przy wejściu
            var currentTheme = Preferences.Get("AppTheme", "System");
            IsSystemTheme = currentTheme == "System";
            IsLightTheme = currentTheme == "Light";
            IsDarkTheme = currentTheme == "Dark";
        }

        // --- ZMIANA MOTYWU ---
        [RelayCommand]
        void ChangeTheme(string themeName)
        {
            Preferences.Set("AppTheme", themeName);
            App.ApplyTheme();
        }

        // --- ZMIANA HASŁA ---
        [RelayCommand]
        async Task ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wpisz nowe hasło", "OK");
                return;
            }

            int userId = Preferences.Get("LoggedUserId", -1);
            string username = Preferences.Get("LoggedUserName", "");

            if (userId != -1)
            {
                var userToUpdate = new Models.User
                {
                    Id = userId,
                    Username = username,
                    // Haszujemy nowe hasło
                    Password = PasswordHasher.HashPassword(NewPassword)
                };

                // ZMIANA: Używamy UpdateUserAsync, bo ta metoda jest w Twoim DatabaseService
                await _databaseService.UpdateUserAsync(userToUpdate);

                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zmienione", "OK");
                NewPassword = string.Empty;
            }
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}