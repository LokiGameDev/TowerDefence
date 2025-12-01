using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridDataSaver : MonoBehaviour
{
    public PlacementSystem placementSystem;
    public ObjectsDatabaseSO database;
    private List<PlacementData> placedObjectsData = new();
    void Start()
    {
        placementSystem.gridData = new GridData();
        placedObjectsData = Load();
    }

    public void LoadTheGridData()
    {
        foreach (var pd in placedObjectsData)
        {
            if (pd == null || pd.occupiedCells == null || pd.occupiedCells.Count == 0)
                continue;

            Debug.Log("Loading object ID: " + pd.ID + " at position: " + pd + " index: " + pd.PlacedObjectIndex);

            // Call methods once per object
            placementSystem.gridData.AddExistingObjectAt(
                pd.occupiedCells, 
                pd.ID, 
                pd.PlacedObjectIndex
            );

            placementSystem.objectPlacer.PlaceObject(
                database.objectsData[pd.ID].prefab,
                placementSystem.grid.CellToWorld(pd.occupiedCells[4])
            );
        }
    }

    public void SaveGridData(PlacementData placementData)
    {
        placedObjectsData.Add(placementData);
        Save(placedObjectsData);
    }

    public void ClearSavedData()
    {
        placedObjectsData.Clear();
        Save(placedObjectsData);
    }

    public void RemoveObjectFromSavedData(int gameObjectIndex)
    {
        for (int i = 0; i < placedObjectsData.Count; i++)
        {
            if (placedObjectsData[i].PlacedObjectIndex == gameObjectIndex)
            {
                GameManager.Instance.inventoryManager.AddItem(placedObjectsData[i].ID, 1);
                placedObjectsData.RemoveAt(i);
            }
        }
        Save(placedObjectsData);
    }

    public void Save(List<PlacementData> placedObjectsData)
    {
        PlacementSaveFile saveFile = new PlacementSaveFile();

        foreach (var pD in placedObjectsData)
        {
            saveFile.entries.Add(new PlacementSaveEntry
            {
                occupiedCells = pD.occupiedCells,
                ID = pD.ID,
                PlacedObjectIndex = pD.PlacedObjectIndex
            });
        }

        string json = JsonUtility.ToJson(saveFile, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", json);

        Debug.Log("Saved!");
    }


    public List<PlacementData> Load()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (!System.IO.File.Exists(path)) return new List<PlacementData>();

        string json = System.IO.File.ReadAllText(path);
        PlacementSaveFile saveFile = JsonUtility.FromJson<PlacementSaveFile>(json);

        List<PlacementData> loadedDict =
            new List<PlacementData>();

        foreach (var entry in saveFile.entries)
        {
            loadedDict.Add(new PlacementData(entry.occupiedCells, entry.ID, entry.PlacedObjectIndex));
        }

        Debug.Log("Loaded!");

        return loadedDict;
    }
}

[System.Serializable]
public class PlacementSaveEntry
{
    public List<Vector3Int> occupiedCells;
    public int ID;
    public int PlacedObjectIndex;
}

[System.Serializable]
public class PlacementSaveFile
{
    public List<PlacementSaveEntry> entries = new List<PlacementSaveEntry>();
}


