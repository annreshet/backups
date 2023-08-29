using Backups.Entities;
using Backups.Extra.RemoveRestorePoint;
using Backups.Interfaces;

namespace Backups.RemoveRestorePoint;

public class RemoveHybrid : IRemoveRestorePoints
{
    private int _amount;
    private DateTime _date;
    private bool _condition;

    public RemoveHybrid(int amount, DateTime date, bool condition)
    {
        _amount = amount;
        _date = date;
        _condition = condition;
    }

    public IReadOnlyCollection<RestorePoint> RestorePointsToRemove(Backup backup)
    {
        var byAmount = new RemoveByAmount(_amount);
        IReadOnlyCollection<RestorePoint> restorePointsToRemoveByAmount = byAmount.RestorePointsToRemove(backup);
        var byDate = new RemoveByDate(_date);
        IReadOnlyCollection<RestorePoint> restorePointsToRemoveByDate = byDate.RestorePointsToRemove(backup);

        List<RestorePoint> restorePointsToRemove = _condition ?
            restorePointsToRemoveByAmount.Intersect(restorePointsToRemoveByDate).ToList() :
            restorePointsToRemoveByAmount.Union(restorePointsToRemoveByDate).ToList();

        return restorePointsToRemove.AsReadOnly();
    }
}