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

        // Lista opcji filtrowania
        public ObservableCollection<FilterOption> FilterOptions { get; } = new();

        // Ukryta lista (cache) wszystkich zadań
        private List<TodoItem> _allTasks = new();

        // --- NOWOŚĆ: Pole wyszukiwania ---
        [ObservableProperty]
        string searchText;

        // Ta metoda uruchomi się automatycznie, gdy wpiszesz choćby jedną literę (dzięki CommunityToolkit)
        partial void OnSearchTextChanged(string value)
        {
            ApplyFilter();
        }

        // Pola formularza
        [ObservableProperty]
        string newTodoTitle;

        [ObservableProperty]
        string newTodoDescription;

        [ObservableProperty]
        DateTime newTodoDate = DateTime.Now;

        // Lista kategorii
        public List<string> Categories { get; } = new()
        {
            "Dom", "Praca", "Szkoła", "Zakupy", "Inne"
        };

        // Wybrana kategoria
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
            {
                FilterOptions.Add(new FilterOption { Name = cat, IsSelected = false });
            }
        }

        [RelayCommand]
        void SelectFilter(FilterOption selectedOption)
        {
            foreach (var option in FilterOptions) option.IsSelected = false;
            selectedOption.IsSelected = true;
            ApplyFilter();
        }

        // --- ZMODYFIKOWANE FILTROWANIE ---
        void ApplyFilter()
        {
            var activeFilter = FilterOptions.FirstOrDefault(f => f.IsSelected)?.Name;

            Tasks.Clear();
            foreach (var task in _allTasks)
            {
                // Warunek 1: Czy pasuje kategoria?
                bool categoryMatches = (activeFilter == "Wszystkie" || task.Category == activeFilter);

                // Warunek 2: Czy pasuje tekst szukania? (ignorujemy wielkość liter)
                // Jeśli SearchText jest pusty, to zwracamy true (pasuje wszystko)
                bool searchMatches = string.IsNullOrWhiteSpace(SearchText)
                                     || task.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

                // Dodajemy tylko jeśli OBA warunki są spełnione
                if (categoryMatches && searchMatches)
                {
                    Tasks.Add(task);
                }
            }
        }

        [RelayCommand]
        async Task LoadTasks()
        {
            int userId = Preferences.Get("LoggedUserId", -1);
            if (userId == -1) return;

            var tasksFromDb = await _databaseService.GetTodosForUserAsync(userId);

            _allTasks = tasksFromDb.OrderBy(t => t.DueDate).ToList();
            ApplyFilter();
        }

        [RelayCommand]
        async Task Tap(TodoItem task)
        {
            if (task == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "TaskObj", task }
            };
            await Shell.Current.GoToAsync(nameof(Views.EditPage), navigationParameter);
        }

        [RelayCommand]
        async Task ToggleDone(TodoItem task)
        {
            if (task == null) return;

            task.IsDone = !task.IsDone;
            await _databaseService.SaveTodoAsync(task);

            // Opcjonalnie: Przeładowujemy, żeby zaktualizować wygląd (np. przekreślenie)
            // Ale w Twoim kodzie XAML masz Triggers, więc powinno działać od razu.
            // Jeśli chcesz, by zadania "wskakiwały" na inne miejsce po zrobieniu, tu można dać LoadTasks().
        }

        [RelayCommand]
        async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTodoTitle))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wpisz tytuł zadania!", "OK");
                return;
            }

            int userId = Preferences.Get("LoggedUserId", -1);
            if (userId == -1)
            {
                await Shell.Current.GoToAsync($"//{nameof(Views.LoginPage)}");
                return;
            }

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
            await LoadTasks();

            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;
            NewTodoDate = DateTime.Now;
            SelectedCategory = "Inne";
        }

        [RelayCommand]
        async Task DeleteTask(TodoItem item)
        {
            if (item == null) return;

            bool answer = await Shell.Current.DisplayAlert("Usuwanie", "Czy na pewno usunąć?", "Tak", "Nie");
            if (!answer) return;

            await _databaseService.DeleteTodoAsync(item);
            _allTasks.Remove(item);
            ApplyFilter();
        }

        [RelayCommand]
        async Task GoToSettings()
        {
            await Shell.Current.GoToAsync(nameof(Views.SettingsPage));
        }

        [RelayCommand]
        async Task Logout()
        {
            Preferences.Remove("LoggedUserId");
            await Shell.Current.GoToAsync($"//{nameof(Views.LoginPage)}");
        }
    }
}