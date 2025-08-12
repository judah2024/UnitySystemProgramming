using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    Stage,
}

public class SceneLoader
{
    public void LoadScene(SceneType sceneType)
    {
        LoggerEx.Log($"{sceneType} scene loading...");
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    public void ReloadScene()
    {
        LoggerEx.Log($"{SceneManager.GetActiveScene().name} scene loading...");
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}