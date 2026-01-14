using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class TodoPage : ContentPage
{
    private readonly TodoViewModel _viewModel;

    public TodoPage(TodoViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel; // £¹czymy widok z logik¹
    }

    // Ta metoda uruchamia siê za ka¿dym razem, gdy wchodzisz na ten ekran
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Wywo³ujemy komendê ³adowania zadañ z bazy
        _viewModel.LoadTasksCommand.Execute(null);
    }
}