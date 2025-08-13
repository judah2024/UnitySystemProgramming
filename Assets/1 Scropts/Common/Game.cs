using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

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

    private void OnApplicationPause(bool isPause)
    {
        if (isPause)
            CurrentSceneController?.OnPause();
        else
            CurrentSceneController?.OnResume();
    }

    private async UniTask InitializeRoutine()
    {
        LoggerEx.Log("[Game] Game Init");
        await InitializeGlobalManager();
    }

    private async UniTask InitializeGlobalManager()
    {
        await UniTask.Yield();
    }

    #region Scene

    private SceneLoader _sceneLoader = new SceneLoader();
    public static SceneLoader SceneLoader => Instance._sceneLoader;
    public ISceneController CurrentSceneController => _sceneLoader.CurrentSceneController;


    #endregion
    
}