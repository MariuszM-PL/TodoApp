using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    // QueryProperty pozwala odebrać dane przesłane z poprzedniego ekranu
    [QueryProperty(nameof(TodoItem), "TaskObj")]
    public partial class EditViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        TodoItem todoItem;

        public EditViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        async Task UpdateTask()
        {
            await _databaseService.SaveTodoAsync(TodoItem);
            await Shell.Current.GoToAsync(".."); // Wróć do listy
        }

        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync(".."); // Wróć bez zapisywania
        }
    }
}