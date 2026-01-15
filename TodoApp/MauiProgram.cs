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
                    //fonts.AddFont("Montserrat-Regular.ttf", "AppFontRegular");
                    //fonts.AddFont("Montserrat-SemiBold.ttf", "AppFontBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Rejestracja Serwisu Bazy Danych
            builder.Services.AddSingleton<DatabaseService>();

            // Rejestracja ViewModeli
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();

            // Rejestracja Stron (Widoków)
            builder.Services.AddTransient<LoginPage>();     
            builder.Services.AddTransient<RegisterPage>();  
            builder.Services.AddTransient<TodoPage>();
            builder.Services.AddTransient<TodoViewModel>();
            builder.Services.AddTransient<EditViewModel>();
            builder.Services.AddTransient<EditPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();
            return builder.Build();
        }
    }
}
