using MauiUltimateTemplate.Domain.Entities;

namespace MauiUltimateTemplate.Domain.Interfaces
{
    public interface ICloudSyncService
    {
        Task<bool> PushToCloudAsync(Note note);
        Task<IEnumerable<Note>> PullFromCloudAsync();
    }
}
