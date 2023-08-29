using Backups.Tools;

namespace Backups.Entities;

public class File
{
    public File(string name, string directoryPath)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Name of file cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new BackupsException("Directory name cannot be empty");
        }

        Name = name;
        DirectoryPath = directoryPath;
        FilePath = Path.Combine(DirectoryPath, Name);
    }

    public string Name { get; }
    public string DirectoryPath { get; }
    public string FilePath { get; }
}