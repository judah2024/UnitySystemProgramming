using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    Stage,
}

/// <summary>
/// 씬 전환기
/// 비동기/동기 씬 전환 지원(예정)
/// </summary>
public class SceneLoader : IDisposable
{
    public ISceneController CurrentSceneController { get; private set; }
    
    
    public SceneLoader()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void Dispose()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        GC.SuppressFinalize(this);
    }

    public void LoadScene(SceneType sceneType)
    {
        LoggerEx.Log($"[SceneLoader] {sceneType} scene loading...");
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    public AsyncOperation LoadSceneAsync(SceneType sceneType)
    {
        LoggerEx.Log($"[SceneLoader] {sceneType} scene async loading...");
        
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }

    public void ReloadScene()
    {
        LoggerEx.Log($"[SceneLoader] {SceneManager.GetActiveScene().name} scene loading...");
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoggerEx.Log($"[SceneLoader] Scene loaded: {scene.name} (Mode: {mode})");
        CurrentSceneController = GameObject.FindAnyObjectByType<SceneControllerBase>();
        CurrentSceneController?.OnEnter();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        LoggerEx.Log($"[SceneLoader] Scene unloaded: {scene.name}");
        CurrentSceneController?.OnExit();
    }

    public void OnPause(bool isPause)
    {
        if (isPause)
            CurrentSceneController?.OnPause();
        else
            CurrentSceneController?.OnResume();
    }
}