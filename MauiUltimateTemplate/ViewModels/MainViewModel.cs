using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MauiUltimateTemplate.Services.DTOs;
using MauiUltimateTemplate.Services.Features;

namespace MauiUltimateTemplate.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly NoteManager _noteManager;

        [ObservableProperty]
        private ObservableCollection<NoteDto> notes;

        public MainViewModel(NoteManager noteManager)
        {
            _noteManager = noteManager;
            //Task.Run(async () => await LoadNotes());
        }

        [RelayCommand]
        private async Task LoadNotes()
        {
            var data = await _noteManager.GetNotesListAsync();
            Notes = new ObservableCollection<NoteDto>(data);
        }

        [RelayCommand]
        private async Task AddNote()
        {
            await _noteManager.CreateNoteAsync("Новая заметка", "Текст заметки...");
            await LoadNotes(); // Обновляем список через бизнес-логику
        }
    }
}
