using Backups.Entities;
using Backups.Interfaces;

namespace Backups.StorageAlgorithms;

public class SplitStorage : IStorageAlgorithms
{
    public IReadOnlyCollection<Storage> CreateStorages(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, int id)
    {
        return backupObjects.Select(backupObject => new Storage(new List<BackupObject>() { backupObject }.AsReadOnly(), directoryPath, id)).ToList().AsReadOnly();
    }
}