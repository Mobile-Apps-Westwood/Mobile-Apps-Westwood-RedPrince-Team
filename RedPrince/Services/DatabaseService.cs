using SQLite;
using RedPrince.Models;

namespace RedPrince.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        private async Task InitAsync()
        {
            if (_database is not null)
                return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "RedPrince.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<User>();
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            await InitAsync();
            return await _database.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            await InitAsync();
            return await _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }


        public async Task<int> CreateUserAsync(User user)
        {
            await InitAsync();
            return await _database.InsertAsync(user);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            await InitAsync();
            return await _database.UpdateAsync(user);
        }
    }
}