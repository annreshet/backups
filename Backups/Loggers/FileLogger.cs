using Backups.Interfaces;
using Backups.Tools;
using Serilog;
using Serilog.Core;

namespace Backups.Loggers;

public class FileLogger : ILogging
{
    public FileLogger(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new BackupsException("Path cannot be empty");
        }

        Path = path;
    }

    public string Path { get; }
    public void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new BackupsException("Log message cannot be empty");
        }

        Logger logger = new LoggerConfiguration().WriteTo.File(Path).CreateLogger();
        logger.Information(message);
    }
}