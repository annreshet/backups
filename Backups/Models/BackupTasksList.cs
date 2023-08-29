using Backups.Entities;

namespace Backups.Models;

public class BackupTasksList
{
    private List<BackupTask> _backupTasks;
    public BackupTasksList()
    {
        _backupTasks = new List<BackupTask>();
    }

    public BackupTasksList(List<BackupTask> backupTasks)
    {
        _backupTasks = backupTasks;
    }

    public IReadOnlyCollection<BackupTask> BackupTasks => _backupTasks.AsReadOnly();

    public void AddBackupTask(BackupTask backupTask)
    {
        _backupTasks.Add(backupTask);
    }

    public void RemoveBackupTask(BackupTask backupTaskExtra)
    {
        _backupTasks.Remove(backupTaskExtra);
    }
}