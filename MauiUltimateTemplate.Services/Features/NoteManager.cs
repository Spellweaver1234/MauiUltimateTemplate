using MauiUltimateTemplate.Domain.Entities;
using MauiUltimateTemplate.Domain.Interfaces;
using MauiUltimateTemplate.Services.DTOs;

using IConnectivity = MauiUltimateTemplate.Domain.Interfaces.IConnectivity;

namespace MauiUltimateTemplate.Services.Features
{
    public class NoteManager
    {
        private readonly INoteRepository _repository;
        private readonly ICloudSyncService _syncService;
        private readonly IConnectivity _connectivity;

        // Мы просим ИНТЕРФЕЙСЫ, а не конкретные классы (DI в действии)
        public NoteManager(
            INoteRepository repository,
            ICloudSyncService syncService,
            IConnectivity connectivity)
        {
            _repository = repository;
            _syncService = syncService;
            _connectivity = connectivity;
        }

        // СЦЕНАРИЙ: Создание заметки с авто-синхронизацией
        public async Task CreateNoteAsync(string title, string content)
        {
            var note = new Note { Title = title, Content = content };

            // 1. Сохраняем локально (Infrastructure сделает это в SQLite)
            await _repository.AddAsync(note);

            // 2. Если есть сеть — пушим в облако
            if (_connectivity.IsConnected)
            {
                var success = await _syncService.PushToCloudAsync(note);
                if (success)
                {
                    note.IsSynced = true;
                    await _repository.UpdateAsync(note);
                }
            }
        }

        // СЦЕНАРИЙ: Получение списка для UI
        public async Task<IEnumerable<NoteDto>> GetNotesListAsync()
        {
            var notes = await _repository.GetAllAsync();
            return notes.Select(n => new NoteDto(
                n.Id,
                n.Title,
                n.Content.Length > 50 ? n.Content[..50] + "..." : n.Content,
                n.CreatedAt.ToShortDateString(),
                n.UpdatedAt.ToShortDateString()
            ));
        }
    }
}
