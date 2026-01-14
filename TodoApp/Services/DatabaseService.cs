using SQLite;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        // Metoda inicjalizująca bazę danych (tworzy plik i tabele, jeśli nie istnieją)
        async Task Init()
        {
            if (_database is not null)
                return;

            // Ścieżka do pliku bazy danych na urządzeniu
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TodoApp.db");

            _database = new SQLiteAsyncConnection(dbPath);

            // Tworzymy tabelę Użytkowników i Zadań
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<TodoItem>();
        }

        // --- CZĘŚĆ 1: UŻYTKOWNICY (Logowanie i Rejestracja) ---

        // Pobierz użytkownika po nazwie (sprawdzenie czy istnieje)
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            await Init();
            return await _database.Table<User>()
                            .Where(u => u.Username == username)
                            .FirstOrDefaultAsync();
        }

        // Logowanie: sprawdź czy pasuje login i hasło
        public async Task<User> LoginUserAsync(string username, string password)
        {
            await Init();
            return await _database.Table<User>()
                            .Where(u => u.Username == username && u.Password == password)
                            .FirstOrDefaultAsync();
        }

        // Rejestracja: dodaj nowego użytkownika
        public async Task RegisterUserAsync(User user)
        {
            await Init();
            await _database.InsertAsync(user);
        }

        // Aktualizacja danych użytkownika (np. zmiana hasła)
        public async Task UpdateUserAsync(User user)
        {
            await Init();
            await _database.UpdateAsync(user);
        }
        // --- CZĘŚĆ 2: ZADANIA (CRUD) ---

        // Pobierz zadania TYLKO dla konkretnego użytkownika (userId)
        public async Task<List<TodoItem>> GetTodosForUserAsync(int userId)
        {
            await Init();
            return await _database.Table<TodoItem>()
                            .Where(t => t.UserId == userId)
                            .ToListAsync();
        }

        // Dodaj lub Zaktualizuj zadanie
        public async Task SaveTodoAsync(TodoItem item)
        {
            await Init();
            if (item.Id != 0)
                await _database.UpdateAsync(item); // Edycja
            else
                await _database.InsertAsync(item); // Nowe
        }

        // Usuń zadanie
        public async Task DeleteTodoAsync(TodoItem item)
        {
            await Init();
            await _database.DeleteAsync(item);
        }
    }
}