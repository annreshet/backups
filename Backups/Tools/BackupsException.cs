namespace Backups.Tools;

public class BackupsException : Exception
{
    public BackupsException(string message)
        : base(message)
    {
    }
}