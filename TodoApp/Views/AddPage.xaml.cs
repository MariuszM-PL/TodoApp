using TodoApp.ViewModels;

namespace TodoApp.Views;

/// <summary>
/// Klasa code-behind dla strony dodawania nowego zadania.
/// Odpowiada za powi¹zanie widoku z g³ównym TodoViewModel i zarz¹dzanie wygl¹dem strony.
/// </summary>
public partial class AddPage : ContentPage
{
    /// <summary>
    /// Konstruktor strony dodawania zadania.
    /// Inicjalizuje komponenty oraz ustawia kontekst bindowania dla MVVM.
    /// </summary>
    /// <param name="vm">Instancja TodoViewModel zarz¹dzaj¹ca logik¹ dodawania zadañ.</param>
    public AddPage(TodoViewModel vm)
    {
        InitializeComponent();

        // Ukrywamy pasek nawigacji, aby zachowaæ spójnoœæ z ekranem edycji
        // i korzystaæ z w³asnego nag³ówka zdefiniowanego w XAML.
        Shell.SetNavBarIsVisible(this, false);

        // Ustawienie kontekstu danych na g³ówny ViewModel, który zawiera 
        // w³aœciwoœci NewTodoTitle, NewTodoDescription oraz komendê AddTaskCommand.
        BindingContext = vm;
    }
}