using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;

namespace Backups.RemoveRestorePoint;

public class RemoveByAmount : IRemoveRestorePoints
{
    private const int MinimalAmount = 1;
    private int _amount;

    public RemoveByAmount(int amount)
    {
        if (amount < MinimalAmount)
        {
            throw new BackupsException("Amount of restore points cannot be zero or negative");
        }

        _amount = amount;
    }

    public IReadOnlyCollection<RestorePoint> RestorePointsToRemove(Backup backup)
    {
        var restorePointsToRemove = new List<RestorePoint>();
        IReadOnlyCollection<RestorePoint> restorePoints = backup.RestorePoints;

        for (int i = 0; i < restorePoints.Count - _amount; ++i)
        {
            restorePointsToRemove.Add(restorePoints.ElementAt(i));
        }

        return restorePointsToRemove.AsReadOnly();
    }
}