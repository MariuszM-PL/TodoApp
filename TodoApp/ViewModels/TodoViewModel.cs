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

        #region Właściwości Nowego Zadania

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

        /// <summary>Data planowanego zadania wybiarana w formularzu.</summary>
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

        /// <summary>Zadanie, które w danej chwili wywołało powiadomienie (używane przez dymek Toast).</summary>
        [ObservableProperty]
        TodoItem currentNotificationTask = default!;

        /// <summary>
        /// Główna pętla sprawdzająca terminy zadań. Uruchamiana przez timer.
        /// </summary>
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

        /// <summary>
        /// Odtwarza plik dźwiękowy przypisany do powiadomienia.
        /// </summary>
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

        /// <summary>
        /// Inicjuje wyświetlenie animowanego dymka powiadomienia w widoku TodoPage.
        /// </summary>
        /// <param name="task">Zadanie, o którym przypominamy.</param>
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

        /// <summary>
        /// Konstruktor ViewModelu inicjalizujący serwisy, filtry oraz timer powiadomień.
        /// </summary>
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

        /// <summary>
        /// Tworzy początkowe opcje filtrowania na podstawie listy kategorii.
        /// </summary>
        void InitializeFilters()
        {
            FilterOptions.Clear();
            FilterOptions.Add(new FilterOption { Name = "Wszystkie", IsSelected = true });
            foreach (var cat in Categories)
                FilterOptions.Add(new FilterOption { Name = cat, IsSelected = false });
        }

        #region Komendy (RelayCommand)

        /// <summary>
        /// Obsługuje wybór filtra kategorii przez użytkownika.
        /// </summary>
        [RelayCommand]
        void SelectFilter(FilterOption selectedOption)
        {
            if (selectedOption == null) return;
            foreach (var option in FilterOptions) option.IsSelected = false;
            selectedOption.IsSelected = true;
            ApplyFilter();
        }

        /// <summary>
        /// Odświeża kolekcję Tasks na podstawie aktualnych kryteriów wyszukiwania i kategorii.
        /// </summary>
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

        /// <summary>
        /// Pobiera listę zadań zalogowanego użytkownika z bazy danych SQLite.
        /// </summary>
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
        /// Dodaje nowe zadanie do bazy danych i aktualizuje listę w pamięci bez mrugania ekranu.
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

            NewTodoTitle = string.Empty;
            NewTodoDescription = string.Empty;
            NewTodoTime = DateTime.Now.TimeOfDay;
            NewTodoDate = DateTime.Now;
        }

        /// <summary>
        /// Trwale usuwa zadanie z bazy danych oraz z aktualnego widoku.
        /// </summary>
        [RelayCommand]
        async Task DeleteTask(TodoItem item)
        {
            if (item == null) return;
            await _databaseService.DeleteTodoAsync(item);
            _allTasks.Remove(item);
            Tasks.Remove(item);
        }

        /// <summary>
        /// Przełącza status zadania (Zrobione/Niezrobione) i aktualizuje bazę danych.
        /// </summary>
        [RelayCommand]
        async Task ToggleDone(TodoItem task)
        {
            if (task == null) return;
            task.IsDone = !task.IsDone;

            if (!task.IsDone && task.DueDate > DateTime.Now)
                task.HasShownNotification = false;

            await _databaseService.SaveTodoAsync(task);
        }

        /// <summary>
        /// Przechodzi do strony edycji wybranego zadania.
        /// </summary>
        [RelayCommand]
        async Task Tap(TodoItem task)
        {
            if (task == null) return;
            var navigationParameter = new Dictionary<string, object> { { "TaskObj", task } };
            await Shell.Current.GoToAsync(nameof(Views.EditPage), navigationParameter);
        }

        /// <summary>Nawiguje do strony ustawień profilu.</summary>
        [RelayCommand]
        async Task GoToSettings() => await Shell.Current.GoToAsync(nameof(Views.SettingsPage));

        /// <summary>Wylogowuje użytkownika i czyści preferencje sesji.</summary>
        [RelayCommand]
        async Task Logout()
        {
            Preferences.Remove("LoggedUserId");
            await Shell.Current.GoToAsync($"//{nameof(Views.LoginPage)}");
        }

        #endregion
    }
}