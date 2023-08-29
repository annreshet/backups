using System.IO.Compression;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Tools;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace Backups.Repository;

public class FileSystemRepository : IRepository
{
    public void CreateDirectory(string path, string name)
    {
        string directoryPath = Path.Combine(path, name);
        if (Directory.Exists(directoryPath))
        {
            throw new BackupsException("Directory with this path already exists");
        }

        Directory.CreateDirectory(directoryPath);
    }

    public void AddFileToDirectory(string directoryPath, string fileName)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new BackupsException("Directory doesn't exist");
        }

        string filePath = Path.Combine(directoryPath, fileName);
        if (File.Exists(filePath))
        {
            throw new BackupsException("File with this name already exists in the directory");
        }

        File.Create(filePath);
    }

    public void DeleteFileFromDirectory(string directoryPath, string fileName)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new BackupsException("Directory doesn't exist");
        }

        string filePath = Path.Combine(directoryPath, fileName);
        if (!File.Exists(filePath))
        {
            throw new BackupsException("File with this name doesn't exist in the directory");
        }

        File.Delete(filePath);
    }

    public void DeleteDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new BackupsException("Directory doesn't exist");
        }

        Directory.Delete(directoryPath);
    }

    public void CreateArchive(IReadOnlyCollection<BackupObject> backupObjects, string directoryPath, string name)
    {
        string archivePath = Path.Combine(directoryPath, name);
        var archive = new ZipArchive(new FileStream(archivePath, FileMode.Create), ZipArchiveMode.Create);
        foreach (BackupObject backupObject in backupObjects)
        {
            string filePath = backupObject.ObjectPath;
            archive.CreateEntryFromFile(filePath, backupObject.Name);
        }

        archive.Dispose();
    }

    public int GetStoragesAmount(string restorePointPath)
    {
        return Directory.GetFiles(restorePointPath).Length;
    }
}