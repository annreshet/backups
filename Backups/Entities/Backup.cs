namespace Backups.Entities;

public class Backup
{
    private readonly List<RestorePoint> _restorePoints;
    public Backup(List<RestorePoint> restorePoints)
    {
        _restorePoints = restorePoints;
    }

    public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Remove(restorePoint);
    }
}