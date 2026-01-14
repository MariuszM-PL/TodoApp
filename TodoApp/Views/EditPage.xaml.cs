using TodoApp.ViewModels;
namespace TodoApp.Views;

public partial class EditPage : ContentPage
{
    public EditPage(EditViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}