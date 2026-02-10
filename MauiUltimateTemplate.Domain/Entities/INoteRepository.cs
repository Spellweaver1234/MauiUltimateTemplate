namespace MauiUltimateTemplate.Domain.Entities
{
    // Интерфейс хранилища (неважно: SQLite это или просто файлы)
    public interface INoteRepository
    {
        Task<Note> GetByIdAsync(Guid id);
        Task<IEnumerable<Note>> GetAllAsync();
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Guid id);
    }
}
