using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneControllerBase : MonoBehaviour, ISceneController
{
    public string Name => gameObject.scene.name;
    
    private bool isInitialized = false;

    protected virtual void Awake()
    {
        if (isInitialized) return;
        
        OnEnter();
    }

    public virtual void OnEnter()
    {
        isInitialized  = true;
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnPause()
    {
    }

    public virtual void OnResume()
    {
    }
}