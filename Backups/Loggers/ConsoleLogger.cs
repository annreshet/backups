using Backups.Interfaces;
using Backups.Tools;
using Serilog;
using Serilog.Core;

namespace Backups.Loggers;

public class ConsoleLogger : ILogging
{
    public void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new BackupsException("Log message cannot be empty");
        }

        Logger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        logger.Information(message);
    }
}