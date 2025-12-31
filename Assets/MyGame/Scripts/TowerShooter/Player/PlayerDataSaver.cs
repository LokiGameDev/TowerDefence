using System.IO;
using UnityEngine;


public static class PlayerDataSaver
{
    public static PlayerData LoadPlayerData()
    {
        string path = PlayerPrefs.GetString("CurrentGamePath") + "/playerData1.dat";
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

    public static PlayerData LoadPlayerDataFromFile(string path)
    {
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
        string path = PlayerPrefs.GetString("CurrentGamePath") + "/playerData1.dat";
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
    }

    public static void SaveShopData(ShopData shopData)
    {
        string json = JsonUtility.ToJson(shopData);
        File.WriteAllText(PlayerPrefs.GetString("CurrentGamePath") + "/shopData1.json", json);
        Debug.Log("Shop data saved");
    }

    public static ShopData LoadShopData()
    {
        if (!File.Exists(PlayerPrefs.GetString("CurrentGamePath") + "/shopData1.json"))
        {
            Debug.Log("Save not found. Creating new data.");
            ShopData shopData = new ShopData();
            SaveShopData(shopData);
            return shopData;
        }

        string json = File.ReadAllText(PlayerPrefs.GetString("CurrentGamePath") + "/shopData1.json");
        return JsonUtility.FromJson<ShopData>(json);
    }

    public static void SaveTowerData(TowerData towerData)
    {
        string json = JsonUtility.ToJson(towerData);
        File.WriteAllText(PlayerPrefs.GetString("CurrentGamePath") + "/towerData.json", json);
        Debug.Log("Shop data saved");
    }

    public static TowerData LoadTowerData()
    {
        if (!File.Exists(PlayerPrefs.GetString("CurrentGamePath") + "/towerData.json"))
        {
            Debug.Log("Save not found. Creating new data.");
            TowerData towerData = new TowerData();
            SaveTowerData(towerData);
            return towerData;
        }

        string json = File.ReadAllText(PlayerPrefs.GetString("CurrentGamePath") + "/towerData.json");
        return JsonUtility.FromJson<TowerData>(json);
    }
}
