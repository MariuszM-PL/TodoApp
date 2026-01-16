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

        // Bezpoœrednie wywo³anie metody asynchronicznej jest pewniejsze ni¿ Execute(null)
        if (_viewModel != null)
        {
            _ = _viewModel.LoadTasks();
        }
    }
}