using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretDataSaver
{
    public static void SaveTurretData(TurretSaveData data)
    {
        string path = PlayerPrefs.GetString("CurrentGamePath") + "/turretData1.json";
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Saved to: " + path);
    }

    public static TurretSaveData LoadTurretData()
    {
        string path = PlayerPrefs.GetString("CurrentGamePath") + "/turretData1.json";
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        TurretSaveData data = JsonUtility.FromJson<TurretSaveData>(json);
        return data;
    }
}
