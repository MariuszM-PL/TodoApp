namespace TodoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // 1. NOWOŚĆ: Ładujemy zapisany motyw OD RAZU przy starcie
            ApplyTheme();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            // Ustawienia rozmiaru okna dla Windows i Mac (Twoje stare ustawienia)
            if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {
                window.Width = 500;  // Szersze
                window.Height = 950; // Wyższe

                // Blokada przed zbyt mocnym zmniejszeniem
                window.MinimumWidth = 450;
                window.MinimumHeight = 700;
            }

            return window;
        }

        // 2. NOWOŚĆ: Metoda statyczna, którą będziemy wołać też z Ustawień
        public static void ApplyTheme()
        {
            // Pobieramy wartość z pamięci (domyślnie "System")
            var theme = Preferences.Get("AppTheme", "System");

            switch (theme)
            {
                case "Light":
                    Application.Current.UserAppTheme = AppTheme.Light;
                    break;
                case "Dark":
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    break;
                default:
                    Application.Current.UserAppTheme = AppTheme.Unspecified; // Systemowy
                    break;
            }
        }
    }
}