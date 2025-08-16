using System;
using UnityEngine;

public class SettingGameData : IGameData
{
    public bool IsMute { get; private set; }
    
    public void SetDefault()
    {
        LoggerEx.Log($"{GetType()}::SetDefaultData");
        IsMute = false;
    }

    public bool Save()
    {
        LoggerEx.Log($"{GetType()}::SaveData");
        bool isCompleted = false;
        try
        {
            PlayerPrefs.SetInt("IsMute", IsMute ? 1 : 0);
            isCompleted = true;

            LoggerEx.Log($"Save IsMute:{IsMute}");
        }
        catch (Exception e)
        {
            LoggerEx.Log("Save failed (" + e.Message + ")");
        }

        return isCompleted;
    }

    public bool Load()
    {
        LoggerEx.Log($"{GetType()}::LoadData");
        bool isCompleted = false;
        try
        {
            IsMute = PlayerPrefs.GetInt("IsMute") != 1;
            isCompleted = true;
            
            LoggerEx.Log($"Load IsMute:{IsMute}");

        }
        catch (Exception e)
        {
            LoggerEx.Log("Load failed (" + e.Message + ")");
        }

        return isCompleted;
    }
}