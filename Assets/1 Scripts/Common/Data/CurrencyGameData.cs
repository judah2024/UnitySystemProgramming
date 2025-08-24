using System;
using UnityEngine;

public class CurrencyGameData : IGameData
{
    public long Gem { get; private set; }
    public long Gold { get; private set; }
    
    public void SetDefault()
    {
        LoggerEx.Log($"{GetType()}::SetDefaultData");
       
        Gem = 0;
        Gold = 0;
    }

    public bool Save()
    {
        LoggerEx.Log($"{GetType()}::SaveData");
        bool isCompleted = false;
        try
        {
            PlayerPrefs.SetString("Gem", Gem.ToString());
            PlayerPrefs.SetString("Gold", Gold.ToString());
            isCompleted = true;

            LoggerEx.Log($"Save Gem:{Gem} Gold:{Gold}");
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
            Gem = long.Parse(PlayerPrefs.GetString("Gem"));
            Gold = long.Parse(PlayerPrefs.GetString("Gold"));
            isCompleted = true;
            
            LoggerEx.Log($"LoadM Gem:{Gem} Gold:{Gold}");

        }
        catch (Exception e)
        {
            LoggerEx.Log("Load failed (" + e.Message + ")");
        }

        return isCompleted;
    }
}