using AutoMapper;

using CommunityToolkit.Maui;

using MauiUltimateTemplate.Application.AutoMapper;
using MauiUltimateTemplate.Application.Managers;
using MauiUltimateTemplate.Application.UI.ViewModels;
using MauiUltimateTemplate.Domain.Interfaces;
using MauiUltimateTemplate.Infrastructure.Device;
using MauiUltimateTemplate.Infrastructure.ExternalServices;
using MauiUltimateTemplate.Infrastructure.Persistence;

using Microsoft.Extensions.Logging;

using IConnectivity = MauiUltimateTemplate.Domain.Interfaces.IConnectivity;

namespace MauiUltimateTemplate
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // 1. Infrastructure (Реализации)
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "notes.db3");
            builder.Services.AddSingleton<INoteRepository>(s => new SqliteNoteRepository(dbPath));
            builder.Services.AddSingleton<ICloudSyncService, GitHubSyncService>();
            builder.Services.AddSingleton<IConnectivity, MauiConnectivityService>();
            builder.Services.AddSingleton<HttpClient>();

            // 2. Application (Бизнес-логика/Use Cases)
            builder.Services.AddTransient<NoteManager>();
            builder.Services.AddSingleton(provider =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });
                return config.CreateMapper();
            });

            // 3. Presentation (UI: ViewModels и Pages)
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
