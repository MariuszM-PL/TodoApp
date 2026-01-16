namespace TodoApp
{
    /// <summary>
    /// Główna klasa aplikacji, odpowiedzialna za inicjalizację cyklu życia programu,
    /// konfigurację okna startowego oraz zarządzanie motywami graficznymi.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Konstruktor klasy App. Inicjalizuje komponenty, nakłada zapisany motyw 
        /// i ustawia stronę główną (AppShell).
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Ładowanie zapisanego motywu użytkownika natychmiast przy uruchomieniu
            ApplyTheme();

            MainPage = new AppShell();
        }

        /// <summary>
        /// Konfiguruje parametry okna aplikacji podczas jego tworzenia.
        /// Odpowiada za centrowanie okna oraz ustawienie stałych wymiarów na systemach Windows i macOS.
        /// </summary>
        /// <param name="activationState">Stan aktywacji przekazany przez system operacyjny.</param>
        /// <returns>Skonfigurowany obiekt Window.</returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            if (window != null)
            {
                // Konfiguracja specyficzna dla platform desktopowych (Windows/Mac)
                if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    // 1. Definiowanie preferowanych wymiarów okna
                    window.Width = 550;
                    window.Height = 780;

                    // 2. Pobieranie informacji o głównym wyświetlaczu
                    var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

                    // 3. Obliczanie pozycji środkowej ekranu z uwzględnieniem gęstości pikseli (skalowania)
                    double x = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
                    double y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;

                    // 4. Ustawienie współrzędnych startowych okna
                    window.X = x;
                    window.Y = y;

                    // 5. Ustawienie minimalnych limitów rozmiaru dla wygody użytkownika
                    window.MinimumWidth = 500;
                    window.MinimumHeight = 750;
                }
            }

            return window ?? new Window();
        }

        /// <summary>
        /// Statyczna metoda umożliwiająca zmianę motywu aplikacji z dowolnego miejsca w kodzie.
        /// Odczytuje preferencje użytkownika i nakłada odpowiedni styl (Jasny/Ciemny/Systemowy).
        /// </summary>
        public static void ApplyTheme()
        {
            // Pobranie klucza motywu z pamięci trwałej (klasa Preferences)
            var theme = Preferences.Get("AppTheme", "System");

            if (Application.Current == null) return;

            // Zastosowanie wybranego motywu graficznego
            switch (theme)
            {
                case "Light":
                    Application.Current.UserAppTheme = AppTheme.Light;
                    break;
                case "Dark":
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    break;
                default:
                    // Powrót do domyślnych ustawień systemu operacyjnego
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
            }
        }
    }
}