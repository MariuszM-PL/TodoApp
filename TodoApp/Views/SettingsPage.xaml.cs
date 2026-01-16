using TodoApp.ViewModels;

namespace TodoApp.Views;

/// <summary>
/// Klasa code-behind dla ekranu ustawieñ aplikacji.
/// Umo¿liwia u¿ytkownikowi personalizacjê interfejsu (motyw) oraz zarz¹dzanie kontem (zmiana has³a).
/// </summary>
public partial class SettingsPage : ContentPage
{
    /// <summary>
    /// Konstruktor strony ustawieñ.
    /// Inicjalizuje komponenty strony, ukrywa pasek nawigacji i przypisuje kontekst danych.
    /// </summary>
    /// <param name="vm">Instancja SettingsViewModel dostarczona przez system wstrzykiwania zale¿noœci.</param>
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();

        // Ukrywamy pasek nawigacji Shell, poniewa¿ strona posiada w³asny przycisk powrotu 
        // zaprojektowany w pliku XAML, co pozwala na lepsz¹ kontrolê nad designem.
        Shell.SetNavBarIsVisible(this, false);

        // Ustawienie powi¹zania z logik¹ SettingsViewModel (MVVM)
        BindingContext = vm;
    }
}