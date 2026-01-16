using SQLite;

namespace TodoApp.Models
{
    /// <summary>
    /// Model reprezentujący konto użytkownika w aplikacji.
    /// Zawiera podstawowe dane uwierzytelniające oraz unikalny identyfikator.
    /// </summary>
    [Table("users")]
    public class User
    {
        /// <summary>
        /// Unikalny identyfikator użytkownika generowany automatycznie przez bazę danych.
        /// Wykorzystywany do relacji z zadaniami (UserId w TodoItem).
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Unikalny login użytkownika. 
        /// Atrybut [Unique] zapewnia, że w bazie nie pojawią się dwa konta o tej samej nazwie.
        /// </summary>
        [Unique]
        public string? Username { get; set; }

        /// <summary>
        /// Skrót (hash) hasła użytkownika.
        /// Ze względów bezpieczeństwa nie przechowujemy tutaj hasła w formie jawnego tekstu.
        /// </summary>
        public string? Password { get; set; }
    }
}