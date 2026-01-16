using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Models
{
    /// <summary>
    /// Reprezentuje pojedyncze zadanie do wykonania w systemie.
    /// Klasa pełni rolę modelu bazodanowego oraz obiektu obserwowanego (MVVM).
    /// </summary>
    [Table("todo_items")]
    public partial class TodoItem : ObservableObject
    {
        /// <summary>Unikalny identyfikator zadania w bazie danych.</summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>Tytuł zadania.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Opcjonalny, rozszerzony opis zadania.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>Status wykonania zadania. Zmiana powiadamia interfejs użytkownika.</summary>
        [ObservableProperty]
        bool isDone;

        /// <summary>Planowana data i godzina realizacji zadania.</summary>
        public DateTime DueDate { get; set; }

        /// <summary>Identyfikator użytkownika, do którego przypisane jest to zadanie.</summary>
        public int UserId { get; set; }

        /// <summary>Kategoria zadania (np. Dom, Praca, Szkoła).</summary>
        public string Category { get; set; } = "Inne";

        /// <summary>
        /// Flaga informująca, czy powiadomienie dźwiękowe dla tego zadania zostało już wyświetlone.
        /// Zapobiega wielokrotnemu odtwarzaniu dźwięku przez Timer.
        /// </summary>
        public bool HasShownNotification { get; set; }

        /// <summary>
        /// Właściwość wyliczana zwracająca kolor przypisany do kategorii zadania.
        /// Pole to jest ignorowane przez bazę danych SQLite.
        /// </summary>
        [Ignore]
        public Color CategoryColor
        {
            get
            {
                return Category switch
                {
                    "Dom" => Color.FromArgb("#4CAF50"),
                    "Praca" => Color.FromArgb("#2196F3"),
                    "Szkoła" => Color.FromArgb("#9C27B0"),
                    "Zakupy" => Color.FromArgb("#FF9800"),
                    _ => Color.FromArgb("#607D8B")
                };
            }
        }
    }
}