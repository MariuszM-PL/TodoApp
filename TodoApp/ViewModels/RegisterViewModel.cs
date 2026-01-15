using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Helpers; // Ważne: to pozwala używać PasswordHasher

namespace TodoApp.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        string username = string.Empty;

        [ObservableProperty]
        string password = string.Empty;

        public RegisterViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        async Task Register()
        {
            // 1. Walidacja: Czy pola nie są puste?
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wprowadź nazwę użytkownika i hasło.", "OK");
                return;
            }

            // 2. Walidacja: Minimalna długość (Twoje wymaganie)
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

            // 3. Sprawdzenie czy użytkownik już istnieje
            // ZMIANA: Używamy metody GetUserByUsernameAsync (tak jak w Twoim serwisie)
            var existingUser = await _databaseService.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await Shell.Current.DisplayAlert("Błąd", "Taki użytkownik już istnieje.", "OK");
                return;
            }

            // 4. Haszowanie hasła i zapis
            var newUser = new User
            {
                Username = Username,
                Password = PasswordHasher.HashPassword(Password) // Haszujemy hasło!
            };

            // ZMIANA: Używamy metody RegisterUserAsync (tak jak w Twoim serwisie)
            await _databaseService.RegisterUserAsync(newUser);

            await Shell.Current.DisplayAlert("Sukces", "Konto utworzone", "OK");

            // Czyścimy pola
            Username = string.Empty;
            Password = string.Empty;

            // Wracamy do logowania
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}