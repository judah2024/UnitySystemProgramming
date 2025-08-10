using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
{
    private static readonly object _lock = new object();
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Logger.LogError($"[Singleton] Instance of {typeof(T)} is not initialized yet.");
            return _instance;
        }
    }

    protected bool _isDontDestroyOnLoad = true;
    
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        lock (_lock)
        {
            if (_instance != null)
            {
                Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} detected. Destroying: {gameObject.name}");
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
            if (_instance == null)
            {
                Debug.LogError($"[Singleton] Failed to cast to {typeof(T)}. Singleton not initialized.");
                Destroy(gameObject);
                return;
            }

            if (_isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        Dispose();
    }

    protected virtual void Dispose()
    {
        _instance = null;
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticInstance()
    {
        _instance = null;
    }
}
