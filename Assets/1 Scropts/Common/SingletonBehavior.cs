using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
{
    protected static T _instance;
    public static T Instance => _instance;

    protected bool _isDontDestroyOnLoad = true;
    
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
        
        if (_isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
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
}
