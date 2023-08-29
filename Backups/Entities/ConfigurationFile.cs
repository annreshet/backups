using System.Text.Json;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities;

public sealed class ConfigurationFile
{
    private const string FilePath = @".\..\..\..\..\..\Lab5\Backups.Extra\configurationFile.json";
    private static ConfigurationFile? _configurationFile;
    private BackupTasksList? _backupTasksList;
    private StreamWriter? _file;

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    private ConfigurationFile()
    {
        if (System.IO.File.Exists(FilePath) && System.IO.File.ReadAllText(FilePath) != string.Empty)
        {
            string json = System.IO.File.ReadAllText(FilePath);
            _backupTasksList = JsonSerializer.Deserialize<BackupTasksList>(json, _options);
        }
        else
        {
            _file = System.IO.File.CreateText(FilePath);
            _file.Close();
            _backupTasksList = new BackupTasksList();
        }
    }

    public static ConfigurationFile GetConfigurationFile()
    {
        return _configurationFile ??= new ConfigurationFile();
    }

    public void Save(BackupTask backupTask)
    {
        BackupTask? backupTaskFromConfig = UploadByName(backupTask.Name);
        if (backupTaskFromConfig is not null)
        {
            _backupTasksList?.RemoveBackupTask(backupTaskFromConfig);
        }

        _backupTasksList?.AddBackupTask(backupTask);
        string newJson = JsonSerializer.Serialize(_backupTasksList, _options);
        _file = System.IO.File.CreateText(FilePath);
        _file.Write(newJson);
        _file.Close();
    }

    public BackupTask FindOrCreateBackupTask(
        string rootPath,
        string name,
        IStorageAlgorithms storageAlgorithm,
        IRepository repository,
        ILogging logger)
    {
        GetConfigurationFile();
        BackupTask? backupTask = UploadByName(name);
        if (backupTask is not null) return backupTask;

        backupTask = new BackupTask(rootPath, name, storageAlgorithm, repository, logger);
        _backupTasksList?.AddBackupTask(backupTask);
        Save(backupTask);

        return backupTask;
    }

    private BackupTask? UploadByName(string backupTaskName)
    {
        BackupTask? backupTask = _backupTasksList?.BackupTasks.FirstOrDefault(backupTask => backupTask.Name == backupTaskName);
        return backupTask;
    }
}