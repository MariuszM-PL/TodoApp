using TodoApp.ViewModels;

namespace TodoApp.Views;

/// <summary>
/// Klasa code-behind dla strony edycji zadania.
/// Odpowiada za powi¹zanie widoku z logik¹ EditViewModel oraz konfiguracjê wygl¹du okna.
/// </summary>
public partial class EditPage : ContentPage
{
    /// <summary>
    /// Konstruktor strony Edycji.
    /// Inicjalizuje komponenty, ukrywa pasek nawigacji i ustawia kontekst bindowania.
    /// </summary>
    /// <param name="vm">Instancja EditViewModel wstrzykniêta przez kontener zale¿noœci.</param>
    public EditPage(EditViewModel vm)
    {
        InitializeComponent();

        // Ukrycie systemowego paska nawigacji (NavBar), aby uzyskaæ 
        // bardziej nowoczesny i spójny wygl¹d interfejsu u¿ytkownika.
        Shell.SetNavBarIsVisible(this, false);

        // Powi¹zanie logiki biznesowej z widokiem (MVVM)
        BindingContext = vm;
    }
}