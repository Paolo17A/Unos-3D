using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Unos/Data/SaveData")]
public class SaveData : ScriptableObject
{
    public string SaveID;

    [Header("PLAYER VARIABLES")]
    public Vector3 PlayerWorldPos;
    public GameManager.Gender PlayerGender;
    public string CurrentScene;

    [Header("CALAMITY VARIABLES")]
    public GameManager.Calamity CurrentCalamity;
    public List<GameManager.Calamity> AccomplishedCalamities;

    [Header("QUEST VARIABLES")]
    public bool DoneWithDrugstore;
    public bool DoneWithHardwareStore;
    public bool DoneWithMarket;

    public void SaveThisData()
    {
        PlayerPrefs.SetString(SaveID, JsonUtility.ToJson(this));
        Debug.Log("Save JSON: " + PlayerPrefs.GetString(SaveID));
    }

    public void LoadFromJsonString(string jsonString)
    {
        try
        {
            SaveDataModel loadedData = JsonUtility.FromJson<SaveDataModel>(jsonString);

            // Assign the loaded data to this instance's fields
            SaveID = loadedData.SaveID;
            PlayerWorldPos = loadedData.PlayerWorldPos;
            PlayerGender = loadedData.PlayerGender;
            CurrentScene = loadedData.CurrentScene;
            CurrentCalamity = loadedData.CurrentCalamity;
            AccomplishedCalamities = new List<GameManager.Calamity>(loadedData.AccomplishedCalamities);
            DoneWithDrugstore = loadedData.DoneWithDrugstore;
            DoneWithHardwareStore = loadedData.DoneWithHardwareStore;
            DoneWithMarket = loadedData.DoneWithMarket;
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data from JSON: " + e.Message);
        }
    }

    public void ResetSaveData()
    {
        PlayerWorldPos = Vector3.zero;
        PlayerGender = GameManager.Gender.NONE;
        CurrentScene = "";
        CurrentCalamity = GameManager.Calamity.NONE;
        AccomplishedCalamities.Clear();
        DoneWithDrugstore = false;
        DoneWithHardwareStore = false;
        DoneWithMarket = false;
    }

    public class SaveDataModel
    {
        public string SaveID;
        public Vector3 PlayerWorldPos;
        public GameManager.Gender PlayerGender;
        public string CurrentScene;
        public GameManager.Calamity CurrentCalamity;
        public List<GameManager.Calamity> AccomplishedCalamities;
        public bool DoneWithDrugstore;
        public bool DoneWithHardwareStore;
        public bool DoneWithMarket;
    }
}
