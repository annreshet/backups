using Backups.Entities;
using Backups.Interfaces;
using Backups.Loggers;
using Backups.Repository;
using Backups.StorageAlgorithms;
using Xunit;

namespace Backups.Test;

public class BackupTests
{
    [Fact]
    public void SingleStorageAlgorithm_InMemoryRepository()
    {
        string rootPath = Path.Combine("C:", "Users", "Anna");
        string backupTaskSingleInMemoryName = "BackupTaskSingleInMemory";
        IStorageAlgorithms singleStorageAlgorithm = new SingleStorage();
        IRepository inMemoryRepository = new InMemoryRepository(rootPath);
        ILogging logger = new ConsoleLogger();
        var backupTaskSingleInMemory = new BackupTask(rootPath, backupTaskSingleInMemoryName, singleStorageAlgorithm, inMemoryRepository, logger);
        backupTaskSingleInMemory.AddBackupObject(rootPath, "FileA");
        backupTaskSingleInMemory.AddBackupObject(rootPath, "FileB");

        RestorePoint restorePoint1 = backupTaskSingleInMemory.CreateRestorePoint();
        string expectedPath = Path.Combine("C:", "Users", "Anna", "BackupTaskSingleInMemory", "RestorePoint1");
        Assert.Equal(expectedPath, restorePoint1.Path);
        Assert.Equal(expectedPath, restorePoint1.Storages.First().DirectoryPath);
        RestorePoint restorePoint2 = backupTaskSingleInMemory.CreateRestorePoint();
        IRepository repository = backupTaskSingleInMemory.Repository;
        Assert.Equal(2, repository.GetStoragesAmount(restorePoint1.Path) + repository.GetStoragesAmount(restorePoint2.Path));
    }
    
    [Fact]
    public void SplitStorageAlgorithm_InMemoryRepository()
    {
        string rootPath = Path.Combine("C:", "Users", "Anna");
        string backupTaskSplitInMemoryName = "BackupTaskSplitInMemory";
        IStorageAlgorithms splitStorageAlgorithm = new SplitStorage();
        IRepository inMemoryRepository = new InMemoryRepository(rootPath);
        ILogging logger = new ConsoleLogger();
        var backupTaskSplitInMemory = new BackupTask(rootPath, backupTaskSplitInMemoryName, splitStorageAlgorithm, inMemoryRepository, logger);
        backupTaskSplitInMemory.AddBackupObject(rootPath, "FileA");
        backupTaskSplitInMemory.AddBackupObject(rootPath, "FileB");

        string backupTaskPath = Path.Combine(backupTaskSplitInMemory.RootPath, backupTaskSplitInMemory.Name);
        string expectedPath = Path.Combine("C:", "Users", "Anna", "BackupTaskSplitInMemory");
        Assert.Equal(expectedPath, backupTaskPath);
        RestorePoint restorePoint1 = backupTaskSplitInMemory.CreateRestorePoint();
        IRepository repository = backupTaskSplitInMemory.Repository;
        Assert.Equal(2, repository.GetStoragesAmount(restorePoint1.Path));
    }
}