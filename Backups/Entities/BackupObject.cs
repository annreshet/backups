using System.Text.Json;
using Backups.Tools;

namespace Backups.Entities;

public class BackupObject
{
    public BackupObject(string directoryPath, string name)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new BackupsException("Path cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Name cannot be empty");
        }

        DirectoryPath = directoryPath;
        Name = name;
        ObjectPath = Path.Combine(DirectoryPath, Name);
        var options = new JsonSerializerOptions { WriteIndented = true };
        BackupObjectInfo = JsonSerializer.Serialize<BackupObject>(this, options);
    }

    public string DirectoryPath { get; }
    public string Name { get; }
    public string ObjectPath { get; }
    public string BackupObjectInfo { get; }
}