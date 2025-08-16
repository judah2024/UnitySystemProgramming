using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public sealed class TitleSceneController : SceneControllerBase
{
    [SerializeField]
    private Animation logoAnim;

    [SerializeField]
    private GameObject titleObj;
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private TextMeshProUGUI loadingProgressText;

    private new void Awake()
    {
        logoAnim.gameObject.SetActive(true);
        titleObj.SetActive(false);    

        base.Awake();
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        OnEnterRoutine().Forget();
    }

    async UniTask OnEnterRoutine()
    {
        Game.Data.LoadData();

        await LoadingRoutine();
    }

    private async UniTask LoadingRoutine()
    {
        LoggerEx.Log($"{GetType()}::OnEnterRoutine");
        
        logoAnim.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(logoAnim.clip.length));
        
        logoAnim.gameObject.SetActive(false);
        titleObj.SetActive(true);

        var asyncOp = Game.SceneLoader.LoadSceneAsync(SceneType.Lobby);
        if(asyncOp == null)
        {
            LoggerEx.Log("Lobby async loading error.");
            return;
        }

        asyncOp.allowSceneActivation = false;

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
                
        while(!asyncOp.isDone)
        {
            loadingSlider.value = Mathf.Lerp(minPercent, 1.0f, asyncOp.progress);
            loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";

            if(asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;
                break;
            }

            await UniTask.NextFrame();
        }
        
        loadingSlider.value = 1f;
        loadingProgressText.text = $"{(int)(loadingSlider.value * 100)}%";
    }
}