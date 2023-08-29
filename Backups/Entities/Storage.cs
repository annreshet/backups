using Backups.Tools;

namespace Backups.Entities;

public class Storage
{
    public Storage(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, int id)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new BackupsException("Directory path cannot be empty");
        }

        if (id <= 0)
        {
            throw new BackupsException("Invalid ID");
        }

        BackupObjects = backupObjects;
        DirectoryPath = directoryPath;
        Id = id;
    }

    public IReadOnlyCollection<BackupObject> BackupObjects { get; }
    public string DirectoryPath { get; }
    public int Id { get; }
}