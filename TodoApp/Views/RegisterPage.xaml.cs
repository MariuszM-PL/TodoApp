using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = vm;
    }
}