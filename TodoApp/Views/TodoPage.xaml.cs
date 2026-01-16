using TodoApp.ViewModels;

namespace TodoApp.Views;

/// <summary>
/// Klasa code-behind g³ównego ekranu listy zadañ.
/// Zawiera logikê animacji interfejsu u¿ytkownika oraz obs³ugê zdarzeñ systemowych.
/// </summary>
public partial class TodoPage : ContentPage
{
    private readonly TodoViewModel _viewModel;

    /// <summary>
    /// Konstruktor strony g³ównej. Inicjalizuje komponenty, ³¹czy logikê z widokiem
    /// oraz konfiguruje subskrypcjê dla komunikatów o powiadomieniach.
    /// </summary>
    /// <param name="vm">Instancja TodoViewModel zarz¹dzaj¹ca danymi zadañ.</param>
    public TodoPage(TodoViewModel vm)
    {
        InitializeComponent();

        _viewModel = vm;
        BindingContext = _viewModel;

        // --- OBS£UGA ANIMACJI POWIADOMIENIA (IN-APP TOAST) ---
        // Subskrybujemy globalny sygna³ "ShowNotification". Dziêki temu ViewModel 
        // nie musi wiedzieæ nic o animacjach, wysy³a tylko informacjê "poka¿ dymek".
        MessagingCenter.Subscribe<TodoViewModel>(this, "ShowNotification", async (sender) =>
        {
            // Wykonujemy animacjê na w¹tku g³ównym (UI Thread)
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                // 1. Animacja wejœcia: dymek wysuwa siê zza górnej krawêdzi do pozycji widocznej.
                // U¿ywamy Easing.CubicOut dla efektu p³ynnego wyhamowania.
                await NotificationToast.TranslateTo(0, 0, 500, Easing.CubicOut);

                // 2. Pauza: dymek pozostaje widoczny przez 4 sekundy, pozwalaj¹c na przeczytanie treœci.
                await Task.Delay(4000);

                // 3. Animacja wyjœcia: dymek chowa siê z powrotem na pozycjê -150 (poza ekran).
                await NotificationToast.TranslateTo(0, -300, 500, Easing.CubicIn);
            });
        });
    }

    /// <summary>
    /// Metoda wywo³ywana przy ka¿dym pojawieniu siê strony na ekranie.
    /// Inicjuje automatyczne odœwie¿enie listy zadañ u¿ytkownika.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            // Wywo³anie asynchroniczne typu "odpal i zapomnij" (fire and forget)
            _ = _viewModel.LoadTasks();
        }
    }

    /// <summary>
    /// Metoda wywo³ywana, gdy u¿ytkownik opuszcza stronê. 
    /// Miejsce na ewentualne czyszczenie zasobów.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // MessagingCenter.Unsubscribe<TodoViewModel>(this, "ShowNotification");
    }
}