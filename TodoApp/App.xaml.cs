namespace TodoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Ładujemy zapisany motyw OD RAZU przy starcie
            ApplyTheme();

            MainPage = new AppShell();
        }

        // Naprawa CS8765: dodanie ? (nullable) do activationState
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            // Naprawa CS8602: Sprawdzamy czy window nie jest null przed ustawieniem właściwości
            if (window != null)
            {
                if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    window.Width = 500;
                    window.Height = 950;

                    window.MinimumWidth = 450;
                    window.MinimumHeight = 700;
                }
            }

            // Naprawa CS8603: zwracamy window lub tworzymy nowe, by uniknąć null
            return window ?? new Window();
        }

        public static void ApplyTheme()
        {
            var theme = Preferences.Get("AppTheme", "System");

            // Naprawa CS8602: bezpieczne sprawdzenie czy Current nie jest null
            if (Application.Current == null) return;

            switch (theme)
            {
                case "Light":
                    Application.Current.UserAppTheme = AppTheme.Light;
                    break;
                case "Dark":
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    break;
                default:
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
            }
        }
    }
}