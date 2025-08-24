using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataManager : IManager
{
    private bool _hasSaveData;
    public List<IGameData> DataList { get; private set; }


    public async UniTask Init()
    {
        DataList = new List<IGameData>()
        {
            new SettingGameData(),
            new CurrencyGameData(),
        };

        await UniTask.Yield();
    }

    public void SetDefaultData()
    {
        foreach (var data in DataList)
        {
            data.SetDefault();
        }
    }

    public void SaveData()
    {
        bool hasFailure = false;
        foreach (var data in DataList)
        {
            bool success = data.Save();
            if (success) continue;
            
            hasFailure = true;
        }

        if (hasFailure) return;

        _hasSaveData = true;
        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        _hasSaveData = PlayerPrefs.GetInt("HasSaveData") == 1;
        if (!_hasSaveData)
        {
            SetDefaultData();
            SaveData();
            return;
        }
            
        foreach (var data in DataList)
        {
            data.Load();
        }
    }
    
    public T GetData<T>() where T : class, IGameData
    {
        foreach (var data in DataList)
        {
            if (data is T findData)
                return findData;
        }

        return null;
    }
}