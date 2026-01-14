using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        string newPassword; // Pole na nowe hasło

        [ObservableProperty]
        string appVersion = "1.0.0";

        public SettingsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        async Task ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wpisz nowe hasło", "OK");
                return;
            }

            // Pobierz ID aktualnego użytkownika
            int userId = Preferences.Get("LoggedUserId", -1);
            string username = Preferences.Get("LoggedUserName", "");

            if (userId != -1)
            {
                // Aktualizujemy użytkownika w bazie
                var userToUpdate = new Models.User
                {
                    Id = userId,
                    Username = username,
                    Password = NewPassword // Zapisujemy nowe hasło
                };

                await _databaseService.UpdateUserAsync(userToUpdate);
                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zmienione", "OK");
                NewPassword = string.Empty; // Czyścimy pole
            }
        }

        [RelayCommand]
        async Task GoBack()
        {
            // ".." oznacza: cofnij się do poprzedniej strony
            await Shell.Current.GoToAsync("..");
        }
    }
}