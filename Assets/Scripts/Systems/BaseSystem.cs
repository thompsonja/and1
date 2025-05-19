using UnityEngine;

public enum LogLevel
{
    NONE = 0,
    DEBUG = 1,
    INFO = 2,
    WARN = 3,
    ERROR = 4,
    FATAL = 5
}

public abstract class BaseSystem<T> : Singleton<T> where T : MonoBehaviour
{
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.INFO;

    private void Log(LogLevel level, string message)
    {
        if (level < MinimumLogLevel) return;

        switch (level)
        {
            case LogLevel.DEBUG:
            case LogLevel.INFO:
                Debug.Log(message);
                break;
            case LogLevel.WARN:
                Debug.LogWarning(message);
                break;
            case LogLevel.ERROR:
            case LogLevel.FATAL:
                Debug.LogError(message);
                break;
            default:
                Debug.Log(message);
                break;
        }
    }

    protected void LogInfo(string message) => Log(LogLevel.INFO, message);
    protected void LogWarn(string message) => Log(LogLevel.WARN, message);
    protected void LogError(string message) => Log(LogLevel.ERROR, message);
    protected void LogFatal(string message) => Log(LogLevel.FATAL, message);

}
