namespace MauiUltimateTemplate.Domain.Entities
{
    public interface ICloudSyncService
    {
        Task<bool> PushToCloudAsync(Note note);
        Task<IEnumerable<Note>> PullFromCloudAsync();
    }
}
