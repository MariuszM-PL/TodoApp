using TodoApp.ViewModels;

namespace TodoApp.Views;

/// <summary>
/// Klasa code-behind dla ekranu logowania.
/// Zarz¹dza inicjalizacj¹ widoku oraz powi¹zaniem go z logik¹ LoginViewModel.
/// </summary>
public partial class LoginPage : ContentPage
{
    /// <summary>
    /// Konstruktor strony logowania.
    /// Inicjalizuje komponenty, konfiguruje pasek nawigacji i ustawia kontekst danych.
    /// </summary>
    /// <param name="vm">ViewModel obs³uguj¹cy proces logowania, wstrzykniêty przez system DI.</param>
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();

        // Ukrywamy systemowy pasek nawigacji, aby zachowaæ estetyczny wygl¹d ekranu startowego.
        Shell.SetNavBarIsVisible(this, false);

        // £¹czymy widok z logik¹ biznesow¹ (Data Binding)
        BindingContext = vm;
    }
}