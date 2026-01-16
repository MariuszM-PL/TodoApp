using SQLite;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database = default!; // Naprawa CS8618

        async Task Init()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TodoApp.db");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<TodoItem>();
        }

        // --- UŻYTKOWNICY ---

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            await Init();
            return await _database.Table<User>()
                            .Where(u => u.Username == username)
                            .FirstOrDefaultAsync();
        }

        public async Task<User> LoginUserAsync(string username, string password)
        {
            await Init();
            return await _database.Table<User>()
                            .Where(u => u.Username == username && u.Password == password)
                            .FirstOrDefaultAsync();
        }

        public async Task RegisterUserAsync(User user)
        {
            await Init();
            await _database.InsertAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await Init();
            await _database.UpdateAsync(user);
        }

        // --- ZADANIA ---

        // Metoda diagnostyczna: Pobiera WSZYSTKO bez filtrowania po UserID
        public async Task<List<TodoItem>> GetAllTodosInternalAsync()
        {
            await Init();
            return await _database.Table<TodoItem>().ToListAsync();
        }

        public async Task<List<TodoItem>> GetTodosForUserAsync(int userId)
        {
            await Init();
            return await _database.Table<TodoItem>()
                            .Where(t => t.UserId == userId)
                            .ToListAsync();
        }

        public async Task SaveTodoAsync(TodoItem item)
        {
            await Init();
            if (item.Id != 0)
                await _database.UpdateAsync(item);
            else
                await _database.InsertAsync(item);
        }

        public async Task DeleteTodoAsync(TodoItem item)
        {
            await Init();
            await _database.DeleteAsync(item);
        }
    }
}