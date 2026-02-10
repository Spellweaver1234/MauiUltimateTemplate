namespace MauiUltimateTemplate.Domain.Entities
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsSynced { get; set; } // Статус синхронизации

        // Бизнес-логика прямо в сущности (валидация)
        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new Exception("Заметка не может быть пустой");

            Content = newContent;
            UpdatedAt = DateTime.UtcNow;
            IsSynced = false;
        }
    }
}
