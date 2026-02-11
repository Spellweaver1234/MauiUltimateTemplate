using AutoMapper;

using MauiUltimateTemplate.Application.AutoMapper;
using MauiUltimateTemplate.Application.Managers;
using MauiUltimateTemplate.Application.UI.ViewModels;
using MauiUltimateTemplate.Infrastructure.Device;
using MauiUltimateTemplate.Infrastructure.ExternalServices;
using MauiUltimateTemplate.Infrastructure.Persistence;

// 1. Создаем инфраструктурные сервисы
var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes_console.db3");
var repository = new SqliteNoteRepository(dbPath);

var syncService = new GitHubSyncService(new HttpClient());
var connectivity = new ConsoleConnectivityService();
var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
var mapper = config.CreateMapper();

// 2. Теперь передаем ВСЕ аргументы в NoteManager
var manager = new NoteManager(repository, syncService, connectivity, mapper);

// 3. Создаем VM
var vm = new MainViewModel(manager);

Console.WriteLine("--- МОИ ЗАМЕТКИ (КОНСОЛЬ) ---");

while (true)
{
    Console.WriteLine("\n1. Показать список | 2. Добавить заметку | 3. Удалить заметку | 0. Выход");
    var choice = Console.ReadLine();

    if (choice == "1")
    {
        // Вызываем команду загрузки вручную
        ShowAll();
    }
    else if (choice == "2")
    {
        Console.Write("Введите заголовок заметки: ");
        vm.NewNoteTitle = Console.ReadLine();

        Console.Write("Введите текст заметки: ");
        vm.NewNoteContent = Console.ReadLine();

        // Вызываем команду — она подхватит данные из свойств выше
        await vm.AddNoteCommand.ExecuteAsync(null);

        Console.WriteLine("\nЗаметка успешно сохранена!");

        ShowAll();
    }
    else if (choice == "3")
    {
        Console.Write("Введите ID заметки для удаления (или часть ID): ");
        string inputId = Console.ReadLine();

        // Ищем заметку в загруженном списке VM (чтобы не заставлять юзера вводить весь Guid)
        var noteToDelete = vm.Notes.FirstOrDefault(n => n.Id.ToString().Contains(inputId));

        if (noteToDelete != null)
        {
            await vm.DeleteNoteCommand.ExecuteAsync(noteToDelete.Id);
            Console.WriteLine("Заметка удалена!");
        }
        else
        {
            Console.WriteLine("Заметка с таким ID не найдена.");
        }

        ShowAll();
    }
    else if (choice == "0") break;
}

async void ShowAll()
{
    await vm.LoadNotesCommand.ExecuteAsync(null);

    foreach (var note in vm.Notes)
    {
        Console.WriteLine($">{note.Id}\t{note.CreatedAt}\t[{note.Title}]: {note.Content}");
    }
}