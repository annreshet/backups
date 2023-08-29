using Backups.Entities;
using Backups.Interfaces;

namespace Backups.StorageAlgorithms;

public class SingleStorage : IStorageAlgorithms
{
    public IReadOnlyCollection<Storage> CreateStorages(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, int id)
    {
        return new List<Storage>() { new (backupObjects, directoryPath, ++id) }.AsReadOnly();
    }
}