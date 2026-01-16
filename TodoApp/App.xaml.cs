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
                    // 1. Ustawiamy wymiary (zmniejszone pod laptopa)
                    window.Width = 450;
                    window.Height = 750;

                    // 2. Pobieramy wymiary wyświetlacza głównego
                    var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

                    // 3. Obliczamy środek (uwzględniając gęstość pikseli/skalowanie)
                    // Dzielimy szerokość ekranu przez gęstość, odejmujemy połowę szerokości okna
                    double x = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
                    double y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;

                    // 4. Ustawiamy pozycję okna
                    window.X = x;
                    window.Y = y;

                    // Blokada minimalnych wymiarów
                    window.MinimumWidth = 400;
                    window.MinimumHeight = 600;
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