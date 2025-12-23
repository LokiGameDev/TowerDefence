using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretDataSaver
{
    static string[] allPath = new string[] {
        PlayerPrefs.GetString("CurrentGamePath") + "/turretData1.json"
    };

    public static void SaveTurretData(TurretSaveData data)
    {
        string path = allPath[GameManager.Instance.saveIndex];
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Saved to: " + path);
    }

    public static TurretSaveData LoadTurretData()
    {
        string path = allPath[GameManager.Instance.saveIndex];
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        TurretSaveData data = JsonUtility.FromJson<TurretSaveData>(json);
        return data;
    }
}
