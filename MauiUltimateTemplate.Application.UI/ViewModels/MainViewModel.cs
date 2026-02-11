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

        [ObservableProperty]
        private ObservableCollection<NoteDto> notes;

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
            await _noteManager.CreateNoteAsync("Новая заметка", "Текст заметки...");
            await LoadNotes();
        }
    }
}
