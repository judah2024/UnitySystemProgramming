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
            lock (Locker)
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
				DontDestroyOnLoad(_instance.gameObject);
            }
        }
                
        return _instance;
    }
    

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        CurrentSceneController?.OnExit();
    }

    private void OnApplicationPause(bool isPause)
    {
        if (isPause)
            CurrentSceneController?.OnPause();
        else
            CurrentSceneController?.OnResume();
    }

    private void Init()
    {
        lock (Locker)
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

        InitializeRoutine().Forget();
    }

    private async UniTask InitializeRoutine()
    {
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