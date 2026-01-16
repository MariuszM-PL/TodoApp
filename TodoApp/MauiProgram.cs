using Microsoft.Extensions.Logging;
using TodoApp.Services;
using TodoApp.ViewModels;
using TodoApp.Views;

namespace TodoApp
{
    public static class MauiProgram
    {
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
            builder.Logging.AddDebug();
#endif
            // 1. Serwis bazy zawsze jako Singleton
            builder.Services.AddSingleton<DatabaseService>();

            // 2. Logowanie i Rejestracja mogą zostać jako Transient (tworzone na nowo przy wejściu)
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();

            // 3. KLUCZOWA ZMIANA: Todo i Ustawienia muszą być Singletonami, 
            // aby trzymały dane i stan w pamięci podczas nawigacji
            builder.Services.AddSingleton<TodoViewModel>();
            builder.Services.AddSingleton<TodoPage>();

            // 4. Edycja i Ustawienia
            builder.Services.AddTransient<EditViewModel>();
            builder.Services.AddTransient<EditPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();

            return builder.Build();
        }
    }
}