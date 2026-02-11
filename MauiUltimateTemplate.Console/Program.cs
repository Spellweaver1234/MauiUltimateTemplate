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
    Console.WriteLine("\n1. Показать список | 2. Добавить заметку | 0. Выход");
    var choice = Console.ReadLine();

    if (choice == "1")
    {
        // Вызываем команду загрузки вручную
        await vm.LoadNotesCommand.ExecuteAsync(null);

        foreach (var note in vm.Notes)
        {
            Console.WriteLine($"> {note.CreatedAt} [{note.Title}]: {note.Summary}");
        }
    }
    else if (choice == "2")
    {
        Console.Write("Введите заголовок: ");
        var title = Console.ReadLine();

        // Здесь можно было бы расширить VM, чтобы она принимала параметры,
        // но для теста вызовем существующую команду:
        await vm.AddNoteCommand.ExecuteAsync(null);
        Console.WriteLine("Заметка добавлена!");
    }
    else if (choice == "0") break;
}