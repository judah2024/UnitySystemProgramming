using UnityEngine;
using Cysharp.Threading.Tasks;

public class Game : MonoBehaviour
{
    private static readonly object Locker = new object();
    
    private static Game _instance;
    public static Game Instance
    {
        get
        {
            return _instance ?? GetInstance();
        }
    }

    private static Game GetInstance()
    {
        _instance = FindAnyObjectByType<Game>();
        if (_instance == null)
        {
            var obj = new GameObject("Game");
            _instance = obj.AddComponent<Game>();
        }
                
        return _instance;
    }
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
        
        InitializeRoutine().Forget();
    }

    private void OnDestroy()
    {
        _sceneLoader?.Dispose();
    }

    private async UniTask InitializeRoutine()
    {
        LoggerEx.Log("[Game] Game Init");
        await InitializeGlobalManager();
    }

    private async UniTask InitializeGlobalManager()
    {
        await Data.Init();
    }

    #region Scene

    private SceneLoader _sceneLoader = new SceneLoader();
    public static SceneLoader SceneLoader => Instance._sceneLoader;

    private void OnApplicationPause(bool isPause)
    {
        _sceneLoader.OnPause(isPause);
    }

    #endregion

    #region Manager

    private DataManager _data = new DataManager();
    public static DataManager Data => Instance._data;

    #endregion
}