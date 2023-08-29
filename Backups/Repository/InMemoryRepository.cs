using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;
using Directory = Backups.Entities.Directory;
using File = Backups.Entities.File;

namespace Backups.Repository;

public class InMemoryRepository : IRepository
{
    private List<Directory> _directories = new ();
    public InMemoryRepository(string rootPath)
    {
        RootPath = rootPath;
        var directory = new Directory(rootPath);
        _directories.Add(directory);
    }

    public IReadOnlyCollection<Directory> Directories => _directories.AsReadOnly();
    public string RootPath { get; }

    public void CreateDirectory(string path, string name)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new BackupsException("Parent directory path cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BackupsException("Name of the directory cannot be empty");
        }

        Directory? parentDirectory = Directories.FirstOrDefault(directory => directory.Path == path);
        if (parentDirectory is null)
        {
            throw new BackupsException("Parent directory doesn't exist");
        }

        string directoryPath = Path.Combine(path, name);
        Directory? directory = Directories.SingleOrDefault(directory => directory.Path == directoryPath);
        if (directory != null)
        {
            throw new BackupsException("Directory with this path already exists");
        }

        directory = new Directory(directoryPath);
        _directories.Add(directory);
    }

    public void AddFileToDirectory(string directoryPath, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new BackupsException("File name cannot be empty");
        }

        Directory? directory = Directories.SingleOrDefault(directory => directory.Path == directoryPath);
        if (directory is null)
        {
            throw new BackupsException("Directory doesn't exist.");
        }

        File? file = directory.Files.SingleOrDefault(file => file.Name == fileName);
        if (file != null)
        {
            throw new BackupsException("File with this name already exists in the directory");
        }

        file = new File(fileName, directoryPath);
        directory.AddFile(file);
    }

    public void DeleteFileFromDirectory(string directoryPath, string fileName)
    {
        Directory? directory = Directories.SingleOrDefault(directory => directory.Path == directoryPath);
        if (directory is null)
        {
            throw new BackupsException("Directory doesn't exist");
        }

        File? file = directory.Files.SingleOrDefault(file => file.Name == fileName);
        if (file is null)
        {
            throw new BackupsException("File with this name doesn't exist in the directory");
        }

        directory.RemoveFile(file);
    }

    public void DeleteDirectory(string directoryPath)
    {
        Directory? directory = Directories.SingleOrDefault(directory => directory.Path == directoryPath);
        if (directory is null)
        {
            throw new BackupsException("Directory doesn't exist");
        }

        _directories.Remove(directory);
    }

    public IReadOnlyCollection<File> GetFilesFromDirectory(string directoryPath)
    {
        Directory? directory = Directories.SingleOrDefault(directory => directory.Path == directoryPath);
        if (directory is null)
        {
            throw new BackupsException("Directory doesn't exist");
        }

        return directory.Files;
    }

    public void CreateArchive(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, string name)
    {
        name += ".zip";
        if (Directories.SingleOrDefault(directory => directory.Path == Path.Combine(directoryPath, name)) is null)
        {
            CreateDirectory(directoryPath, name);
        }

        string archivePath = Path.Combine(directoryPath, name);
        foreach (BackupObject backupObject in backupObjects)
        {
            AddFileToDirectory(archivePath, backupObject.Name);
        }
    }

    public int GetStoragesAmount(string restorePointPath)
    {
        return Directories.Count(directory =>
            directory.Path.StartsWith(restorePointPath) && directory.Path.EndsWith(".zip"));
    }
}