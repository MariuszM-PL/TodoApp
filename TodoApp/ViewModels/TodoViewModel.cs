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

        public ObservableCollection<TodoItem> Tasks { get; } = new();
        public ObservableCollection<FilterOption> FilterOptions { get; } = new();
        private List<TodoItem> _allTasks = new();

        [ObservableProperty]
        string searchText = string.Empty;

        partial void OnSearchTextChanged(string value) => ApplyFilter();

        [ObservableProperty]
        string newTodoTitle = string.Empty;

        [ObservableProperty]
        string newTodoDescription = string.Empty;

        [ObservableProperty]
        DateTime newTodoDate = DateTime.Now;

        public List<string> Categories { get; } = new() { "Dom", "Praca", "Szkoła", "Zakupy", "Inne" };

        [ObservableProperty]
        string selectedCategory = "Inne";

        public TodoViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            InitializeFilters();
        }

        void InitializeFilters()
        {
            FilterOptions.Clear();
            FilterOptions.Add(new FilterOption { Name = "Wszystkie", IsSelected = true });
            foreach (var cat in Categories)
                FilterOptions.Add(new FilterOption { Name = cat, IsSelected = false });
        }

        [RelayCommand]
        void SelectFilter(FilterOption selectedOption)
        {
            if (selectedOption == null) return;
            foreach (var option in FilterOptions) option.IsSelected = false;
            selectedOption.IsSelected = true;
            ApplyFilter();
        }

        void ApplyFilter()
        {
            var activeFilter = FilterOptions.FirstOrDefault(f => f.IsSelected)?.Name ?? "Wszystkie";

            Tasks.Clear();
            foreach (var task in _allTasks)
            {
                bool categoryMatches = (activeFilter == "Wszystkie" || task.Category == activeFilter);
                bool searchMatches = string.IsNullOrWhiteSpace(SearchText)
                                     || (task.Title?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);

                if (categoryMatches && searchMatches)
                {
                    Tasks.Add(task);
                }
            }
        }

        [RelayCommand]
        public async Task LoadTasks()
        {
            int userId = Preferences.Get("LoggedUserId", -1);

            // Próba 1: Pobierz zadania dla użytkownika
            var tasksFromDb = await _databaseService.GetTodosForUserAsync(userId);

            // Próba 2: Jeśli lista jest pusta, pobierz WSZYSTKO (diagnostyka)
            if (tasksFromDb == null || tasksFromDb.Count == 0)
            {
                tasksFromDb = await _databaseService.GetAllTodosInternalAsync();
            }

            _allTasks = (tasksFromDb ?? new List<TodoItem>()).OrderBy(t => t.DueDate).ToList();

            MainThread.BeginInvokeOnMainThread(() => {
                ApplyFilter();
            });
        }

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
                UserId = userId,
                Category = SelectedCategory
            };

            await _databaseService.SaveTodoAsync(newTask);

            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;

            // Powrót do widoku ogólnego
            var allFilter = FilterOptions.FirstOrDefault(f => f.Name == "Wszystkie");
            if (allFilter != null)
            {
                foreach (var option in FilterOptions) option.IsSelected = false;
                allFilter.IsSelected = true;
            }

            await LoadTasks();
        }

        [RelayCommand]
        async Task DeleteTask(TodoItem item)
        {
            if (item == null) return;
            await _databaseService.DeleteTodoAsync(item);
            await LoadTasks();
        }

        [RelayCommand]
        async Task ToggleDone(TodoItem task)
        {
            if (task == null) return;

            // 1. Zmieniamy stan w pamięci. 
            // Dzięki temu, że w TodoItem masz [ObservableProperty] przy IsDone,
            // interfejs użytkownika (UI) zareaguje natychmiastowo na zmianę,
            // a Triggery w XAML zadziałają płynnie bez odświeżania całej listy.
            task.IsDone = !task.IsDone;

            // 2. Zapisujemy zmianę w bazie danych.
            await _databaseService.SaveTodoAsync(task);

            // 3. KLUCZOWA ZMIANA: Usunęliśmy ApplyFilter().
            // Teraz lista nie mruga, bo nie jest czyszczona.
            // Jedyny wyjątek: jeśli masz wybrany filtr, który powinien ukryć wykonane zadanie,
            // wtedy musisz wywołać ApplyFilter(), ale stracisz płynność.
            // Przy widoku "Wszystkie" – zostawiamy tak jak poniżej:
        }

        [RelayCommand]
        async Task Tap(TodoItem task)
        {
            if (task == null) return;
            var navigationParameter = new Dictionary<string, object> { { "TaskObj", task } };
            await Shell.Current.GoToAsync(nameof(Views.EditPage), navigationParameter);
        }

        [RelayCommand]
        async Task GoToSettings() => await Shell.Current.GoToAsync(nameof(Views.SettingsPage));

        [RelayCommand]
        async Task Logout()
        {
            Preferences.Remove("LoggedUserId");
            await Shell.Current.GoToAsync($"//{nameof(Views.LoginPage)}");
        }
    }
}