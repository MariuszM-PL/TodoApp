using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using TodoApp.Services;
using TodoApp.ViewModels;
using TodoApp.Views;

namespace TodoApp
{
    /// <summary>
    /// Klasa startowa aplikacji odpowiedzialna za konfigurację frameworka MAUI,
    /// rejestrację czcionek, serwisów oraz zarządzanie wstrzykiwaniem zależności (Dependency Injection).
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Tworzy i konfiguruje instancję aplikacji MAUI.
        /// </summary>
        /// <returns>Skonfigurowany obiekt MauiApp.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            // Dodaje logowanie błędów do okna Output w Visual Studio podczas debugowania
            builder.Logging.AddDebug();
#endif

            // --- REJESTRACJA SERWISÓW (Dependency Injection) ---

            /// <summary>
            /// DatabaseService jako Singleton - jedna współdzielona instancja bazy 
            /// danych dostępna dla całej aplikacji.
            /// </summary>
            builder.Services.AddSingleton<DatabaseService>();

            /// <summary>
            /// Rejestracja AudioManagera do obsługi dźwięków powiadomień.
            /// </summary>
            builder.Services.AddSingleton(AudioManager.Current);

            // --- REJESTRACJA WIDOKÓW I VIEWMODELI ---

            /// <summary>
            /// Logowanie i Rejestracja (Transient) - obiekty tworzone na nowo przy każdym wejściu,
            /// co zapewnia czysty stan formularzy po powrocie do tych ekranów.
            /// </summary>
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();

            /// <summary>
            /// Strona główna i Ustawienia (Singleton) - obiekty żyją przez cały czas działania aplikacji.
            /// Dzięki temu lista zadań i stan filtrów nie są przeładowywane przy każdym przełączeniu ekranu.
            /// </summary>
            builder.Services.AddSingleton<TodoViewModel>();
            builder.Services.AddSingleton<TodoPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();

            /// <summary>
            /// Edycja (Transient) - strona edycji jest tworzona na nowo dla każdego zadania osobno.
            /// </summary>
            builder.Services.AddTransient<EditViewModel>();
            builder.Services.AddTransient<EditPage>();

            return builder.Build();
        }
    }
}