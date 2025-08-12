using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static readonly object _locker = new object();
    
    private static Game _instance;
    public static Game Instance
    {
        get
        {
            lock (_locker)
            {
                return GetInstance();
            }
        }
    }

    private static Game GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindAnyObjectByType<Game>();
            if (_instance == null)
            {
                var obj = new GameObject("Game");
                _instance = obj.AddComponent<Game>();
            }
        }
                
        return _instance;
    }
    

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        lock (_locker)
        {
            if (_instance == this)
                return;
            
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoggerEx.Log("[Game] Game Init");
        }

        InitializeRoutine();
    }

    private async UniTaskVoid InitializeRoutine()
    {
        await InitializeGlobalManager();
    }

    private async UniTask InitializeGlobalManager()
    {
        
    }

    #region Manager

    private SceneLoader _sceneLoader;
    
    public static SceneLoader SceneLoader => Instance._sceneLoader;

    #endregion
}