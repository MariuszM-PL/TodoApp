using TodoApp.ViewModels;

namespace TodoApp.Views;

/// <summary>
/// Klasa code-behind dla ekranu rejestracji nowego u¿ytkownika.
/// Odpowiada za inicjalizacjê interfejsu oraz powi¹zanie go z logik¹ RegisterViewModel.
/// </summary>
public partial class RegisterPage : ContentPage
{
    /// <summary>
    /// Konstruktor strony rejestracji.
    /// Konfiguruje widok i przypisuje dostarczony ViewModel jako Ÿród³o danych dla bindowania.
    /// </summary>
    /// <param name="vm">ViewModel obs³uguj¹cy rejestracjê, automatycznie dostarczony przez kontener MAUI.</param>
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();

        // Wy³¹czenie widocznoœci paska nawigacji (NavBar) dla uzyskania 
        // nowoczesnego wygl¹du formularza rejestracyjnego.
        Shell.SetNavBarIsVisible(this, false);

        // Powi¹zanie widoku z logik¹ (BindingContext) - serce wzorca MVVM
        BindingContext = vm;
    }
}