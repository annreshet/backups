using Backups.Entities;
using Directory = Backups.Entities.Directory;
using File = Backups.Entities.File;

namespace Backups.Interfaces;

public interface IRepository
{
    void CreateDirectory(string path, string name);
    void AddFileToDirectory(string directoryPath, string fileName);
    void DeleteFileFromDirectory(string directoryPath, string fileName);
    void DeleteDirectory(string directoryPath);
    void CreateArchive(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, string name);
    int GetStoragesAmount(string restorePointPath);
}