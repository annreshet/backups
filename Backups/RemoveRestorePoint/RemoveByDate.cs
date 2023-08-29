using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.Extra.RemoveRestorePoint;

public class RemoveByDate : IRemoveRestorePoints
{
    private DateTime _now = DateTime.Now;
    private DateTime _date;

    public RemoveByDate(DateTime date)
    {
        if (date > _now)
        {
            throw new BackupsException("Cannot restore points from the future");
        }

        _date = date;
    }

    public IReadOnlyCollection<RestorePoint> RestorePointsToRemove(Backup backup)
    {
        IReadOnlyCollection<RestorePoint> restorePoints = backup.RestorePoints;
        var restorePointsToRemove = restorePoints.Where(restorePoint => restorePoint.Date < _date).ToList();

        return restorePointsToRemove.AsReadOnly();
    }
}