using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MauiUltimateTemplate.Application.DTOs;
using MauiUltimateTemplate.Application.Managers;

namespace MauiUltimateTemplate.Application.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly NoteManager _noteManager;

        [ObservableProperty] private ObservableCollection<NoteDto> notes;
        [ObservableProperty] private string newNoteTitle;
        [ObservableProperty] private string newNoteContent;

        public MainViewModel(NoteManager noteManager)
        {
            _noteManager = noteManager;
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
            var title = string.IsNullOrWhiteSpace(NewNoteTitle) ? "Без названия" : NewNoteTitle;
            var content = string.IsNullOrWhiteSpace(NewNoteContent) ? "" : NewNoteContent;

            await _noteManager.CreateNoteAsync(title, content);

            // Очищаем поля после добавления
            NewNoteTitle = string.Empty;
            NewNoteContent = string.Empty;

            await LoadNotes();
        }

        [RelayCommand]
        private async Task DeleteNote(Guid id)
        {
            var success = await _noteManager.RemoveNoteAsync(id);
            if (success)
            {
                // Обновляем список, чтобы удаленная заметка исчезла
                await LoadNotes();
            }
        }
    }
}
