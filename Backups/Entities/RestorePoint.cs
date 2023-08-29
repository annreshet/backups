using System.Text.Json;
using Backups.Tools;

namespace Backups.Entities;

public class RestorePoint
{
    public RestorePoint(string path, DateTime date, IReadOnlyCollection<Storage> storages, IReadOnlyCollection<BackupObject> backupObjects)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new BackupsException("Path cannot be empty");
        }

        Path = path;
        Date = date;
        Storages = storages;
        BackupObjects = backupObjects;
        var options = new JsonSerializerOptions { WriteIndented = true };
        RestorePointInfo = JsonSerializer.Serialize<RestorePoint>(this, options);
    }

    public string Path { get; }
    public DateTime Date { get; }
    public IReadOnlyCollection<Storage> Storages { get; }
    public IReadOnlyCollection<BackupObject> BackupObjects { get; }
    public string RestorePointInfo { get; }
}