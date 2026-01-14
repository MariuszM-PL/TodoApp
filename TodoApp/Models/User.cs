using SQLite;

namespace TodoApp.Models
{
    [Table("users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique] // Nazwa użytkownika nie może się powtarzać
        public string? Username { get; set; }
        public string? Password { get; set; } // W prawdziwej aplikacji hasła się hashuje, na zaliczenie tekst jawny wystarczy
    }
}
