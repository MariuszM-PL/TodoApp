using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TodoApp.Models;
using TodoApp.Services;
using Plugin.Maui.Audio;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel obsługujący główną logikę listy zadań, w tym filtrowanie,
    /// dodawanie nowych elementów oraz autorski system powiadomień dźwiękowych.
    /// </summary>
    public partial class TodoViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly IAudioManager _audioManager;
        private readonly IDispatcherTimer _notificationTimer;

        /// <summary>Lista zadań wyświetlana bezpośrednio w interfejsie użytkownika.</summary>
        public ObservableCollection<TodoItem> Tasks { get; } = new();

        /// <summary>Lista opcji filtrowania kategorii (np. Dom, Praca).</summary>
        public ObservableCollection<FilterOption> FilterOptions { get; } = new();

        /// <summary>Prywatny bufor przechowujący wszystkie zadania pobrane z bazy danych.</summary>
        private List<TodoItem> _allTasks = new();

        #region Właściwości Wyszukiwania i Nowego Zadania

        /// <summary>Tekst wpisany w wyszukiwarkę zadań.</summary>
        [ObservableProperty]
        string searchText = string.Empty;

        /// <summary>Metoda wywoływana automatycznie przy zmianie tekstu wyszukiwania.</summary>
        partial void OnSearchTextChanged(string value) => ApplyFilter();

        /// <summary>Tytuł nowego zadania wpisywany w formularzu.</summary>
        [ObservableProperty]
        string newTodoTitle = string.Empty;

        /// <summary>Opis nowego zadania wpisywany w formularzu.</summary>
        [ObservableProperty]
        string newTodoDescription = string.Empty;

        /// <summary>Data planowanego zadania wybierana w formularzu.</summary>
        [ObservableProperty]
        DateTime newTodoDate = DateTime.Now;

        private TimeSpan _newTodoTime = DateTime.Now.TimeOfDay;
        /// <summary>Godzina planowanego zadania wybierana w formularzu.</summary>
        public TimeSpan NewTodoTime
        {
            get => _newTodoTime;
            set => SetProperty(ref _newTodoTime, value);
        }

        /// <summary>Wybrana kategoria dla nowego zadania.</summary>
        [ObservableProperty]
        string selectedCategory = "Inne";

        /// <summary>Stała lista dostępnych kategorii zadań.</summary>
        public List<string> Categories { get; } = new() { "Dom", "Praca", "Szkoła", "Zakupy", "Inne" };

        #endregion

        #region System Powiadomień Wewnętrznych

        [ObservableProperty]
        TodoItem currentNotificationTask = default!;

        private async Task CheckNotificationsAsync()
        {
            var now = DateTime.Now;
            var tasksToNotify = _allTasks.Where(t =>
                !t.IsDone &&
                !t.HasShownNotification &&
                t.DueDate <= now &&
                t.DueDate > now.AddMinutes(-1)).ToList();

            foreach (var task in tasksToNotify)
            {
                task.HasShownNotification = true;
                await _databaseService.SaveTodoAsync(task);
                await PlayNotificationSound();
                await ShowInAppNotification(task);
            }
        }

        private async Task PlayNotificationSound()
        {
            try
            {
                var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("notification.wav"));
                player.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio Error: {ex.Message}");
            }
        }

        private async Task ShowInAppNotification(TodoItem task)
        {
            CurrentNotificationTask = task;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(this, "ShowNotification");
            });
            await Task.CompletedTask;
        }

        #endregion

        public TodoViewModel(DatabaseService databaseService, IAudioManager audioManager)
        {
            _databaseService = databaseService;
            _audioManager = audioManager;
            InitializeFilters();

            _notificationTimer = Application.Current.Dispatcher.CreateTimer();
            _notificationTimer.Interval = TimeSpan.FromSeconds(5);
            _notificationTimer.Tick += async (s, e) => await CheckNotificationsAsync();
            _notificationTimer.Start();
        }

        void InitializeFilters()
        {
            FilterOptions.Clear();
            FilterOptions.Add(new FilterOption { Name = "Wszystkie", IsSelected = true });
            foreach (var cat in Categories)
                FilterOptions.Add(new FilterOption { Name = cat, IsSelected = false });
        }

        #region Komendy (RelayCommand)

        /// <summary>Nawiguje do strony dodawania nowego zadania.</summary>
        [RelayCommand]
        async Task GoToAddPage() => await Shell.Current.GoToAsync(nameof(Views.AddPage));

        /// <summary>Powraca do poprzedniej strony (np. po kliknięciu Anuluj lub Zapisz).</summary>
        [RelayCommand]
        async Task GoBack() => await Shell.Current.GoToAsync("..");

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
                    Tasks.Add(task);
            }
        }

        [RelayCommand]
        public async Task LoadTasks()
        {
            int userId = Preferences.Get("LoggedUserId", -1);
            var tasksFromDb = await _databaseService.GetTodosForUserAsync(userId);

            if (tasksFromDb == null || tasksFromDb.Count == 0)
                tasksFromDb = await _databaseService.GetAllTodosInternalAsync();

            _allTasks = (tasksFromDb ?? new List<TodoItem>()).OrderBy(t => t.DueDate).ToList();

            MainThread.BeginInvokeOnMainThread(() => {
                ApplyFilter();
            });
        }

        /// <summary>
        /// Dodaje nowe zadanie, czyści formularz i wraca do listy głównej.
        /// </summary>
        [RelayCommand]
        async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTodoTitle)) return;

            DateTime combinedDueDate = NewTodoDate.Date.Add(NewTodoTime);
            var newTask = new TodoItem
            {
                Title = NewTodoTitle,
                Description = NewTodoDescription,
                DueDate = combinedDueDate,
                IsDone = false,
                UserId = Preferences.Get("LoggedUserId", -1),
                Category = SelectedCategory,
                HasShownNotification = false
            };

            await _databaseService.SaveTodoAsync(newTask);

            _allTasks.Add(newTask);
            _allTasks = _allTasks.OrderBy(t => t.DueDate).ToList();
            ApplyFilter();

            // Resetowanie pól
            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;
            NewTodoTime = DateTime.Now.TimeOfDay;
            NewTodoDate = DateTime.Now;
            SelectedCategory = "Inne";

            // Automatyczny powrót do listy
            await GoBack();
        }

        [RelayCommand]
        async Task DeleteTask(TodoItem item)
        {
            if (item == null) return;
            await _databaseService.DeleteTodoAsync(item);
            _allTasks.Remove(item);
            Tasks.Remove(item);
        }

        [RelayCommand]
        async Task ToggleDone(TodoItem task)
        {
            if (task == null) return;
            task.IsDone = !task.IsDone;
            if (!task.IsDone && task.DueDate > DateTime.Now)
                task.HasShownNotification = false;

            await _databaseService.SaveTodoAsync(task);
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

        #endregion
    }
}