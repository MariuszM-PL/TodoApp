using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = vm; // £¹czymy widok z logik¹
    }
}