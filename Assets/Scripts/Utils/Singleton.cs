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

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private LogLevel minimumLogLevel = LogLevel.INFO;
    private string instanceName;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
    }

    // Implement in subclasses to enforce ordering at the GameSystem level
    public virtual void Init(string instanceName, LogLevel level)
    {
        minimumLogLevel = level;
        this.instanceName = instanceName;
        LogInfo($"{instanceName}: Init start");
    }

    protected void InitComplete()
    {
        LogInfo($"{instanceName}: Init complete");
        Initialized = true;
    }

    public virtual void Stop()
    {
        Instance = null;
        Destroy(gameObject);
    }

    protected bool Initialized;

    private void Log(LogLevel level, string message)
    {
        if (level < minimumLogLevel) return;

        switch (level)
        {
            case LogLevel.DEBUG:
            case LogLevel.INFO:
                Debug.Log($"{instanceName}: {message}");
                break;
            case LogLevel.WARN:
                Debug.LogWarning($"{instanceName}: {message}");
                break;
            case LogLevel.ERROR:
            case LogLevel.FATAL:
                Debug.LogError($"{instanceName}: {message}");
                break;
            default:
                Debug.Log($"{instanceName}: {message}");
                break;
        }
    }

    protected void LogInfo(string message) => Log(LogLevel.INFO, message);
    protected void LogWarn(string message) => Log(LogLevel.WARN, message);
    protected void LogError(string message) => Log(LogLevel.ERROR, message);
    protected void LogFatal(string message) => Log(LogLevel.FATAL, message);
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}