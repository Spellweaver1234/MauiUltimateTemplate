using MauiUltimateTemplate.Domain.Entities;
using MauiUltimateTemplate.Domain.Interfaces;

using SQLite;

namespace MauiUltimateTemplate.Infrastructure.Persistence
{
    public class SqliteNoteRepository : INoteRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public SqliteNoteRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            // Создаем таблицу, если её нет
            _database.CreateTableAsync<Note>().Wait();
        }

        public async Task<Note> GetByIdAsync(Guid id) =>
            await _database.Table<Note>().FirstOrDefaultAsync(n => n.Id == id);

        public async Task AddAsync(Note note) => await _database.InsertAsync(note);

        public async Task UpdateAsync(Note note) => await _database.UpdateAsync(note);

        public async Task<IEnumerable<Note>> GetAllAsync() => await _database.Table<Note>().ToListAsync();

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Вместо автоматического метода используем явный запрос
            var count = await _database.ExecuteAsync("DELETE FROM Note WHERE Id = ?", id);
            return count > 0;
        }
    }
}
