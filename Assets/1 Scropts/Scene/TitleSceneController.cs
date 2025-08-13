using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TitleSceneController : SceneControllerBase
{
    [SerializeField]
    private Animation logoAnim;
    [SerializeField]
    private TextMeshProUGUI logoText;

    [SerializeField]
    private GameObject titleObj;
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private TextMeshProUGUI loadingProgressText;
    
    private AsyncOperation _asyncOperation;
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        OnEnterRoutine().Forget();
    }

    async UniTask OnEnterRoutine()
    {
        LoggerEx.Log($"{{GetType()}}::OnEnterRoutine");
        
        logoAnim.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(logoAnim.clip.length));
        
        logoAnim.gameObject.SetActive(false);
        titleObj.SetActive(true);

        Game.SceneLoader.LoadScene(SceneType.Lobby);

        _asyncOperation = Game.SceneLoader.LoadSceneAsync(SceneType.Lobby);
        if(_asyncOperation == null)
        {
            LoggerEx.Log("Lobby async loading error.");
            return;
        }

        _asyncOperation.allowSceneActivation = false;

        loadingSlider.value = 0.5f;
        loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        while(!_asyncOperation.isDone)
        {
            loadingSlider.value = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
            loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";

            if(_asyncOperation.progress >= 0.9f)
            {
                _asyncOperation.allowSceneActivation = true;
                return;
            }

            await UniTask.NextFrame();
        }
    }
}