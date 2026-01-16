using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Models
{
    [Table("todo_items")]
    public partial class TodoItem : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ObservableProperty]
        bool isDone;

        public DateTime DueDate { get; set; }
        public int UserId { get; set; }

        public string Category { get; set; } = "Inne";

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