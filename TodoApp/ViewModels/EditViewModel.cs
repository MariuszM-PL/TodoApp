using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    // QueryProperty odbiera obiekt zadania przekazany z listy
    [QueryProperty(nameof(TodoItem), "TaskObj")]
    public partial class EditViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        TodoItem todoItem = default!;

        public List<string> Categories { get; } = new()
        {
            "Dom", "Praca", "Szkoła", "Zakupy", "Inne"
        };

        public EditViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        async Task UpdateTask()
        {
            // Zabezpieczenie przed nullem
            if (TodoItem == null) return;

            // --- WALIDACJA (Punkt B) ---
            // Sprawdzamy, czy tytuł nie został wyczyszczony
            if (string.IsNullOrWhiteSpace(TodoItem.Title))
            {
                await Shell.Current.DisplayAlert("Błąd", "Tytuł zadania nie może być pusty!", "OK");
                return; // Przerywamy funkcję, nie zapisujemy w bazie
            }

            // Jeśli jest OK, zapisujemy zmiany w bazie
            await _databaseService.SaveTodoAsync(TodoItem);

            // Wracamy do poprzedniego ekranu
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task Cancel()
        {
            // Wracamy bez zapisywania
            await Shell.Current.GoToAsync("..");
        }
    }
}