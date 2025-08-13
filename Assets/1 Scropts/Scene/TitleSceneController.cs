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

        _asyncOperation = Game.SceneLoader.LoadSceneAsync(SceneType.Lobby);
        if(_asyncOperation == null)
        {
            LoggerEx.Log("Lobby async loading error.");
            return;
        }

        _asyncOperation.allowSceneActivation = false;

        loadingSlider.value = 0f;
        loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";

        const float minDelay = 2.0f;
        const float minPercent = 0.9f;
        float time = 0f;
        while (time < minDelay)
        {
            loadingSlider.value = time / minDelay * minPercent;
            loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";
            await UniTask.NextFrame();
                    
            time += Time.unscaledDeltaTime;
        }
                
        while(!_asyncOperation.isDone)
        {
            loadingSlider.value = Mathf.Lerp(minPercent, 1.0f, _asyncOperation.progress);
            loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";

            if(_asyncOperation.progress >= 0.9f)
            {
                _asyncOperation.allowSceneActivation = true;
                break;
            }

            await UniTask.NextFrame();
        }
        
        loadingSlider.value = 1f;
        loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";
    }
}