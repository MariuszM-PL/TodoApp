using TodoApp.Views;

namespace TodoApp
{
    /// <summary>
    /// Klasa definiująca strukturę nawigacyjną aplikacji oraz rejestrująca trasy (routing).
    /// </summary>
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // --- REJESTRACJA TRAS (ROUTING) ---
            // Rejestrujemy strony, które nie są bezpośrednio widoczne w menu głównym (Flyout),
            // ale do których chcemy nawigować programowo za pomocą metody Shell.Current.GoToAsync().

            // Trasa do strony rejestracji - pozwala na przejście z ekranu logowania.
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

            // Trasa do strony edycji - używana, gdy użytkownik chce zmodyfikować istniejące zadanie.
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

            // Trasa do strony ustawień - pozwala na nawigację do opcji konfiguracji aplikacji.
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}