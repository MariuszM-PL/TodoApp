using SQLite;
using TodoApp.Models;

namespace TodoApp.Services
{
    /// <summary>
    /// Serwis zarządzający lokalną bazą danych SQLite.
    /// Odpowiada za przechowywanie danych użytkowników oraz ich zadań (Todo).
    /// </summary>
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database = default!;

        /// <summary>
        /// Inicjalizuje połączenie z bazą danych i tworzy tabele, jeśli jeszcze nie istnieją.
        /// Zastosowano wzorzec Lazy Initialization, aby baza była otwierana tylko wtedy, gdy jest potrzebna.
        /// </summary>
        private async Task Init()
        {
            if (_database is not null)
                return;

            // Ścieżka do pliku bazy danych w bezpiecznym folderze aplikacji
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TodoApp.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // Automatyczne tworzenie tabel na podstawie modeli
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<TodoItem>();
        }

        #region Zarządzanie Użytkownikami

        /// <summary>
        /// Pobiera użytkownika z bazy danych na podstawie jego unikalnej nazwy (loginu).
        /// Wykorzystywane przy logowaniu oraz sprawdzaniu dostępności loginu przy rejestracji.
        /// </summary>
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            await Init();
            return await _database.Table<User>()
                            .Where(u => u.Username == username)
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Weryfikuje poświadczenia użytkownika (używane w starszych wersjach logowania).
        /// </summary>
        public async Task<User> LoginUserAsync(string username, string password)
        {
            await Init();
            return await _database.Table<User>()
                            .Where(u => u.Username == username && u.Password == password)
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Rejestruje nowego użytkownika w systemie.
        /// </summary>
        public async Task RegisterUserAsync(User user)
        {
            await Init();
            await _database.InsertAsync(user);
        }

        /// <summary>
        /// Aktualizuje dane istniejącego użytkownika (np. przy zmianie hasła).
        /// </summary>
        public async Task UpdateUserAsync(User user)
        {
            await Init();
            await _database.UpdateAsync(user);
        }

        #endregion

        #region Zarządzanie Zadaniami (Todo)

        /// <summary>
        /// Pobiera wszystkie zadania ze wszystkich kont (używane diagnostycznie).
        /// </summary>
        public async Task<List<TodoItem>> GetAllTodosInternalAsync()
        {
            await Init();
            return await _database.Table<TodoItem>().ToListAsync();
        }

        /// <summary>
        /// Pobiera listę zadań przypisanych do konkretnego użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator zalogowanego użytkownika.</param>
        public async Task<List<TodoItem>> GetTodosForUserAsync(int userId)
        {
            await Init();
            return await _database.Table<TodoItem>()
                            .Where(t => t.UserId == userId)
                            .ToListAsync();
        }

        /// <summary>
        /// Zapisuje zadanie w bazie danych. Jeśli zadanie posiada ID, następuje aktualizacja,
        /// w przeciwnym razie tworzony jest nowy rekord.
        /// </summary>
        public async Task SaveTodoAsync(TodoItem item)
        {
            await Init();
            if (item.Id != 0)
                await _database.UpdateAsync(item);
            else
                await _database.InsertAsync(item);
        }

        /// <summary>
        /// Trwale usuwa zadanie z bazy danych.
        /// </summary>
        public async Task DeleteTodoAsync(TodoItem item)
        {
            await Init();
            await _database.DeleteAsync(item);
        }

        #endregion
    }
}