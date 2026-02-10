using MauiUltimateTemplate.Domain.Entities;

namespace MauiUltimateTemplate.Domain.Services
{
    public class NoteSyncConflictResolver
    {
        public Note Resolve(Note local, Note remote)
        {
            // Логика: побеждает та заметка, у которой дата UpdatedAt свежее
            return local.UpdatedAt > remote.UpdatedAt ? local : remote;
        }
    }
}
