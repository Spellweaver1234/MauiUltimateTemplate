namespace MauiUltimateTemplate.Application.DTOs
{
    public record NoteDto
    {
        // Пустой конструктор для AutoMapper
        public NoteDto() { }

        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string CreatedAt { get; init; }
        public string UpdatedAt { get; init; }
    }
}
