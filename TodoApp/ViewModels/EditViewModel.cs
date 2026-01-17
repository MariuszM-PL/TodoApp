using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel odpowiedzialny za logikę edycji istniejącego zadania.
    /// Obsługuje przekazywanie danych między widokami oraz aktualizację bazy danych.
    /// </summary>
    [QueryProperty(nameof(TodoItem), "TaskObj")]
    public partial class EditViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        /// <summary>
        /// Obecnie edytowane zadanie, przekazane jako parametr nawigacji.
        /// </summary>
        [ObservableProperty]
        TodoItem todoItem = default!;

        // ===================================================================
        // ZMIANA: Definicja ręczna, żeby naprawić błąd "SelectedCategory does not exist"
        // ===================================================================
        private string _selectedCategory;

        /// <summary>
        /// Wybrana kategoria w Pickerze.
        /// </summary>
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        // ===================================================================

        private TimeSpan _editTodoTime;
        /// <summary>
        /// Pomocnicza właściwość przechowująca czas (godzinę) zadania podczas edycji.
        /// </summary>
        public TimeSpan EditTodoTime
        {
            get => _editTodoTime;
            set => SetProperty(ref _editTodoTime, value);
        }

        /// <summary>
        /// Lista dostępnych kategorii do wyboru w Pickerze.
        /// </summary>
        public List<string> Categories { get; } = new()
        {
            "Dom", "Praca", "Szkoła", "Zakupy", "Inne"
        };

        /// <summary>
        /// Konstruktor inicjalizujący serwis bazy danych.
        /// </summary>
        public EditViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Wywoływane automatycznie, gdy obiekt TodoItem zostanie przypisany do ViewModelu.
        /// Synchronizuje czas zadania z właściwością EditTodoTime oraz odświeża Picker.
        /// </summary>
        partial void OnTodoItemChanged(TodoItem value)
        {
            if (value != null)
            {
                // Synchronizacja godziny z kontrolką TimePicker
                EditTodoTime = value.DueDate.TimeOfDay;

                // Przepisujemy kategorię z obiektu zadania do właściwości SelectedCategory
                if (!string.IsNullOrEmpty(value.Category))
                {
                    SelectedCategory = value.Category;
                }
                else
                {
                    // Jeśli zadanie nie ma kategorii, ustawiamy domyślną
                    SelectedCategory = Categories.FirstOrDefault();
                }

                // Powiadomienie UI o zmianie obiektu
                OnPropertyChanged(nameof(TodoItem));
            }
        }

        /// <summary>
        /// Zapisuje zmiany wprowadzone w zadaniu do bazy danych.
        /// </summary>
        [RelayCommand]
        async Task UpdateTask()
        {
            if (TodoItem == null) return;

            // Walidacja danych wejściowych
            if (string.IsNullOrWhiteSpace(TodoItem.Title))
            {
                await Shell.Current.DisplayAlert("Błąd", "Tytuł zadania nie może być pusty!", "OK");
                return;
            }

            // Połączenie wybranej daty z wybraną godziną
            TodoItem.DueDate = TodoItem.DueDate.Date.Add(EditTodoTime);

            // PRZEPISANIE KATEGORII Z POWROTEM DO ZADANIA
            TodoItem.Category = SelectedCategory;

            // LOGIKA POWIADOMIEŃ:
            if (TodoItem.DueDate > DateTime.Now)
            {
                TodoItem.HasShownNotification = false;
            }

            // Zapis do bazy SQLite
            await _databaseService.SaveTodoAsync(TodoItem);

            // Powrót do poprzedniej strony
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Przerywa edycję i wraca do listy zadań bez zapisywania zmian.
        /// </summary>
        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}