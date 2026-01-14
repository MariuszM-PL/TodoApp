using TodoApp.Views;

namespace TodoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rejestrujemy trasę do strony rejestracji,
            // aby Shell wiedział gdzie iść po wywołaniu "GoToAsync(nameof(RegisterPage))"
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}