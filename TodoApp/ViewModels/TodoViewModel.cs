using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public partial class TodoViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        // Lista zadań widoczna na ekranie
        public ObservableCollection<TodoItem> Tasks { get; } = new();

        // Pola formularza
        [ObservableProperty]
        string newTodoTitle;

        [ObservableProperty]
        string newTodoDescription; // <--- NOWE: Pole na opis

        [ObservableProperty]
        DateTime newTodoDate = DateTime.Now; // Domyślna data to "dzisiaj"

        public TodoViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Uruchamiane przy starcie ekranu
        [RelayCommand]
        async Task LoadTasks()
        {
            int userId = Preferences.Get("LoggedUserId", -1);
            if (userId == -1) return;

            var tasksFromDb = await _databaseService.GetTodosForUserAsync(userId);

            // NOWOŚĆ: Sortowanie (OrderBy)
            // Układa zadania od najstarszej daty (najbliższy termin) do najnowszej
            var sortedTasks = tasksFromDb.OrderBy(t => t.DueDate).ToList();

            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        // Ta metoda zadziała, gdy klikniesz w zadanie (żeby je edytować)
        [RelayCommand]
        async Task Tap(TodoItem task)
        {
            if (task == null) return;

            // Przechodzimy do strony edycji i przekazujemy jej kliknięte zadanie
            var navigationParameter = new Dictionary<string, object>
    {
        { "TaskObj", task }
    };
            await Shell.Current.GoToAsync(nameof(Views.EditPage), navigationParameter);
        }

        // Ta metoda zadziała, gdy klikniesz Checkbox (Zrobione/Niezrobione)
        [RelayCommand]
        async Task ToggleDone(TodoItem task)
        {
            if (task == null) return;

            // <--- NAPRAWA: Musimy ręcznie odwrócić wartość (zrobione <-> niezrobione)
            task.IsDone = !task.IsDone;

            // Teraz zapisujemy nową wartość do bazy
            await _databaseService.SaveTodoAsync(task);
        }

        // Dodawanie zadania
        [RelayCommand]
        async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTodoTitle)) return;

            int userId = Preferences.Get("LoggedUserId", -1);

            var newTask = new TodoItem
            {
                Title = NewTodoTitle,
                Description = NewTodoDescription,
                DueDate = NewTodoDate,
                IsDone = false,
                UserId = userId
            };

            await _databaseService.SaveTodoAsync(newTask);

            // ZMIANA: Zamiast po prostu dodawać na koniec (Tasks.Add),
            // wywołujemy LoadTasks(), żeby odświeżyć listę i zachować sortowanie.
            await LoadTasks();

            // Reset pól
            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;
            NewTodoDate = DateTime.Now;
        }

        // Usuwanie zadania
        [RelayCommand]
        async Task DeleteTask(TodoItem task)
        {
            if (task == null) return;
            await _databaseService.DeleteTodoAsync(task);
            Tasks.Remove(task);
        }

        // Ustawienia
        [RelayCommand]
        async Task GoToSettings()
        {
            await Shell.Current.GoToAsync(nameof(Views.SettingsPage));
        }

        // Wylogowanie
        [RelayCommand]
        async Task Logout()
        {
            Preferences.Remove("LoggedUserId");
            await Shell.Current.GoToAsync($"//{nameof(Views.LoginPage)}");
        }
    }
}