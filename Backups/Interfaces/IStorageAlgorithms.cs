using Backups.Entities;

namespace Backups.Interfaces;

public interface IStorageAlgorithms
{
    IReadOnlyCollection<Storage> CreateStorages(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, int id);
}