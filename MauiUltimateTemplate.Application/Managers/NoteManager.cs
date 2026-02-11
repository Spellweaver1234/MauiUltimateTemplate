using AutoMapper;

using MauiUltimateTemplate.Application.DTOs;
using MauiUltimateTemplate.Domain.Entities;
using MauiUltimateTemplate.Domain.Interfaces;

using IConnectivity = MauiUltimateTemplate.Domain.Interfaces.IConnectivity;

namespace MauiUltimateTemplate.Application.Managers
{
    public class NoteManager
    {
        private readonly INoteRepository _repository;
        private readonly ICloudSyncService _syncService;
        private readonly IConnectivity _connectivity;
        private readonly IMapper _mapper;

        // Мы просим ИНТЕРФЕЙСЫ, а не конкретные классы (DI в действии)
        public NoteManager(
            INoteRepository repository,
            ICloudSyncService syncService,
            IConnectivity connectivity,
            IMapper mapper)
        {
            _repository = repository;
            _syncService = syncService;
            _connectivity = connectivity;
            _mapper = mapper;
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

            return _mapper.Map<List<NoteDto>>(notes);
        }
    }
}
