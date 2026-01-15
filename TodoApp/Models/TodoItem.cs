using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Models
{
    [Table("todo_items")]
    public partial class TodoItem : ObservableObject // <--- 1. Dziedziczenie po ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        // 2. Zmieniamy zwykłe pole na "ObservableProperty".
        // Biblioteka sama wygeneruje publiczną właściwość "IsDone" (dużą literą).
        [ObservableProperty]
        bool isDone;

        public DateTime DueDate { get; set; }
        public int UserId { get; set; }

        // NOWE POLE: Kategoria (zapisywana w bazie)
        public string Category { get; set; }

        // NOWE POLE POMOCNICZE: Kolor (niezapisywany w bazie, obliczany w locie)
        [Ignore]
        public Color CategoryColor
        {
            get
            {
                return Category switch
                {
                    "Dom" => Color.FromArgb("#4CAF50"),      // Zielony
                    "Praca" => Color.FromArgb("#2196F3"),    // Niebieski
                    "Szkoła" => Color.FromArgb("#9C27B0"),   // Fioletowy
                    "Zakupy" => Color.FromArgb("#FF9800"),   // Pomarańczowy
                    "Inne" => Color.FromArgb("#607D8B"),     // Szary
                    _ => Color.FromArgb("#607D8B")           // Domyślny (Szary)
                };
            }
        }
    }
}