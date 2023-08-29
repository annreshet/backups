using Backups.Tools;

namespace Backups.Entities;

public class Directory
{
    private readonly List<File> _files = new ();
    public Directory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new BackupsException("Directory path cannot be empty");
        }

        Path = path;
    }

    public string Path { get; }
    public IReadOnlyCollection<File> Files => _files.AsReadOnly();

    public void AddFile(File file)
    {
        _files.Add(file);
    }

    public void RemoveFile(File file)
    {
        _files.Remove(file);
    }
}