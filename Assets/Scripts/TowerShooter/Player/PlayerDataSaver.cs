using System.IO;
using UnityEngine;


public static class PlayerDataSaver
{
    static string[] allPath = new string[] {
        Application.persistentDataPath + "/playerData1.dat",
        Application.persistentDataPath + "/playerData2.dat",
        Application.persistentDataPath + "/playerData3.dat"
    };

    public static PlayerData LoadPlayerData()
    {
        string path = allPath[GameManager.Instance.saveIndex];
        if (!File.Exists(path)) return null;

        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            PlayerData data = new PlayerData();

            data.playerScore   = reader.ReadInt32();
            data.waveNumber  = reader.ReadInt32();
            data.towerHealth   = reader.ReadInt32();
            
            int dictCount = reader.ReadInt32();

            for (int i = 0; i < dictCount; i++)
            {
                int key = reader.ReadInt32();
                int value = reader.ReadInt32();
                data.inventoryItems[key] = value;
            }

            return data;
        }
    }


    public static void SavePlayerData(PlayerData data)
    {
        string path = allPath[GameManager.Instance.saveIndex];
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(data.playerScore);
            writer.Write(data.waveNumber);
            writer.Write(data.towerHealth);

            writer.Write(data.inventoryItems.Count);

            // Write each key-value pair
            foreach (var kv in data.inventoryItems)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value);
            }
        }

        //Debug.Log("Saved Player Data!");
    }
}
