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
    }
}