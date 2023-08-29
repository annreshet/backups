using Backups.Entities;

namespace Backups.Interfaces;

public interface IRemoveRestorePoints
{
    public IReadOnlyCollection<RestorePoint> RestorePointsToRemove(Backup backup);
}