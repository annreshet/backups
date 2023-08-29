using Backups.Interfaces;
using Backups.RemoveRestorePoint;
using Backups.StorageAlgorithms;
using Backups.Tools;

namespace Backups.Entities;

public class BackupTask
{
    private readonly List<BackupObject> _backupObjects;
    public BackupTask(
        string rootPath,
        string name,
        IStorageAlgorithms storageAlgorithm,
        IRepository repository,
        ILogging logger)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            throw new BackupsException("Path cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Name cannot be empty");
        }

        RootPath = rootPath;
        Name = name;
        _backupObjects = new List<BackupObject>();
        Backup = new Backup(new List<RestorePoint>() { });
        StorageAlgorithm = storageAlgorithm;
        Repository = repository;
        Repository.CreateDirectory(RootPath, Name);
        Logger = logger;
    }

    public string RootPath { get; }
    public string Name { get; }
    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects;
    public Backup Backup { get; }
    public IStorageAlgorithms StorageAlgorithm { get; }
    public IRepository Repository { get; }
    public ILogging Logger { get; }

    public void AddBackupObject(string directoryPath, string name)
    {
        var backupObject = new BackupObject(directoryPath, name);
        _backupObjects.Add(backupObject);
        Repository.AddFileToDirectory(directoryPath, name);
        Logger.Log($"Backup object was added to BackupTask {Name}");
        Logger.Log(backupObject.BackupObjectInfo);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        _backupObjects.Remove(backupObject);
    }

    public RestorePoint CreateRestorePoint()
    {
        string backupTaskPath = Path.Combine(RootPath, Name);
        int restorePointsAmount = Backup.RestorePoints.Count + 1;
        string restorePointName = $"RestorePoint{restorePointsAmount}";
        Repository.CreateDirectory(backupTaskPath, restorePointName);

        string restorePointPath = Path.Combine(backupTaskPath, restorePointName);
        int storagesAmount = Repository.GetStoragesAmount(restorePointPath) + 1;
        IReadOnlyCollection<Storage> storages = StorageAlgorithm.CreateStorages(_backupObjects, restorePointPath, storagesAmount);
        var restorePoint = new RestorePoint(restorePointPath, DateTime.Now, storages, BackupObjects);
        foreach (Storage storage in restorePoint.Storages)
        {
            Repository.CreateArchive(storage.BackupObjects, restorePointPath, $"Storage{storagesAmount++}");
        }

        Backup.AddRestorePoint(restorePoint);
        
        Logger.Log("Restore point was created");
        Logger.Log(restorePoint.RestorePointInfo);

        return restorePoint;
    }
    
    public void RestoreToOriginalLocation(RestorePoint restorePoint)
    {
        foreach (BackupObject backupObject in restorePoint.BackupObjects)
        {
            try
            {
                Repository.AddFileToDirectory(backupObject.DirectoryPath, backupObject.Name);
            }
            catch (BackupsException)
            {
            }
        }

        Logger.Log("Files restored to original location");
    }
    
    public void RestoreToDifferentLocation(RestorePoint restorePoint, string directoryPath)
    {
        foreach (BackupObject backupObject in restorePoint.BackupObjects)
        {
            try
            {
                Repository.AddFileToDirectory(directoryPath, backupObject.Name);
            }
            catch (BackupsException)
            {
            }
        }

        Logger.Log($"Files restored to {directoryPath}");
    }
    
    public void RemoveRestorePointsByAmount(int amount)
    {
        var removeRestorePoints = new RemoveByAmount(amount);
        RemoveRestorePoints(removeRestorePoints);
    }
    
    public void RemoveRestorePointsHybrid(int amount, DateTime date, bool condition)
    {
        var removeRestorePoints = new RemoveHybrid(amount, date, condition);
        RemoveRestorePoints(removeRestorePoints);
    }
    
    private IReadOnlyCollection<RestorePoint> Merge(IReadOnlyCollection<RestorePoint> restorePointsToRemove)
    {
        RestorePoint lastRestorePoint = restorePointsToRemove.ElementAt(restorePointsToRemove.Count - 1);
        RestorePoint nextToLastRestorePoint = restorePointsToRemove.ElementAt(restorePointsToRemove.Count - 2);
        if (StorageAlgorithm.GetType() == typeof(SingleStorage))
        {
            Backup.RemoveRestorePoint(nextToLastRestorePoint);
            Logger.Log("Restore points were merged");
            return Backup.RestorePoints;
        }

        var backupObjects =
            (IReadOnlyCollection<BackupObject>)lastRestorePoint.BackupObjects.Union(
                nextToLastRestorePoint.BackupObjects);
        var storages = (IReadOnlyCollection<Storage>)lastRestorePoint.Storages.Union(nextToLastRestorePoint.Storages);
        string path = lastRestorePoint.Path;
        DateTime date = lastRestorePoint.Date;
        Backup.RemoveRestorePoint(lastRestorePoint);
        Backup.RemoveRestorePoint(nextToLastRestorePoint);
        Backup.AddRestorePoint(new RestorePoint(path, date, storages, backupObjects));
        Logger.Log("Restore points were merged");
        return Backup.RestorePoints;
    }
    
    private void RemoveRestorePoints(IRemoveRestorePoints removeRestorePoints)
    {
        IReadOnlyCollection<RestorePoint> restorePointsToRemove = removeRestorePoints.RestorePointsToRemove(Backup);
        if (restorePointsToRemove.Count == Backup.RestorePoints.Count)
        {
            restorePointsToRemove = Merge(restorePointsToRemove);
        }

        foreach (RestorePoint restorePoint in restorePointsToRemove)
        {
            Backup.RemoveRestorePoint(restorePoint);
        }

        Logger.Log("Restore points were removed");
    }
}