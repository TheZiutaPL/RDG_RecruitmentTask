using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public struct SaveUnit : IComparer<SaveUnit>
{
    public string name;
    public float time;
    public int coins;
    public int deaths;

    public SaveUnit(float time, int coins, int deaths)
    {
        string playerName = PlayerSaveName.PlayerName;

        name = string.IsNullOrWhiteSpace(playerName) ? "Unnamed Player" : playerName;
        this.time = time;
        this.coins = coins;
        this.deaths = deaths;
    }

    public int Compare(SaveUnit x, SaveUnit y)
    {
        return x.time.CompareTo(y.time);
    }
}

public static class SaveSystem
{
    [Serializable]
    public class SaveWrapper
    {
        [NonSerialized] private int lastAddedSaveUnitIndex = -1;
        public int GetLastAddedSaveUnitIndex()
        {
            int value = lastAddedSaveUnitIndex;
            lastAddedSaveUnitIndex = -1;
            return value;
        }

        [SerializeField] private List<SaveUnit> saveUnits = new List<SaveUnit>();
        public List<SaveUnit> GetSaveUnits() => new List<SaveUnit>(saveUnits);
        public void AddSaveUnit(SaveUnit saveUnit)
        {
            int newSaveIndex = 0;

            //Adds new unit and sorts them
            bool added = false;
            for (int i = 0; i < saveUnits.Count; i++)
            {
                //If new saveUnit has a better time than saveUnit[i]
                if(saveUnit.time < saveUnits[i].time)
                {
                    newSaveIndex = i;
                    saveUnits.Insert(i, saveUnit);
                    added = true;
                    break;
                }
            }

            //If it didn't insert it already, it adds saveUnit to the end of the list
            if (!added)
            {
                newSaveIndex = saveUnits.Count;
                saveUnits.Add(saveUnit);
            }               

            lastAddedSaveUnitIndex = newSaveIndex;
        }
    }

    private static SaveWrapper saveFile;
    public static SaveWrapper SaveFile 
    { 
        get 
        {
            if (saveFile == null)
                saveFile = LoadSave();

            return saveFile;
        } 
    }

    private const string FILE_NAME = "save";
    private static string GetFilePath() => Application.persistentDataPath + "/" + FILE_NAME;

    private static SaveWrapper LoadSave()
    {
        string path = GetFilePath();

        //If there is no save file
        if (!File.Exists(path))
            return new SaveWrapper();

        //Creates a BinaryFormatter instance and opens a FileStream
        BinaryFormatter binaryFormatter = new BinaryFormatter();        
        FileStream fileStream = new FileStream(path, FileMode.Open);

        //Reads save file
        SaveWrapper loadedSave = (SaveWrapper)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();

        return loadedSave;
    }

    public static void SaveData()
    {
        string path = GetFilePath();

        //Creates a BinaryFormatter instance and opens a FileStream
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);

        //Saves data
        binaryFormatter.Serialize(fileStream, SaveFile);
        fileStream.Close();
    }
}
