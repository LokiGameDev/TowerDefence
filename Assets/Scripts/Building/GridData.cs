using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjectsData = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2 objectSize, int id, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData placementData = new(positionsToOccupy, id, placedObjectIndex);
        foreach (var position in positionsToOccupy)
        {
            if (placedObjectsData.ContainsKey(position))
            {
                throw new Exception($"Cell {position} is already occupied.");
            }
            placedObjectsData[position] = placementData;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2 objectSize)
    {
        List<Vector3Int> positions = new();
        // for (int x = 0; x < objectSize.x; x++)
        // {
        //     for (int z = 0; z < objectSize.y; z++)
        //     {
        //         positions.Add(gridPosition + new Vector3Int(x, 0, z));
        //     }
        // }

        int halfSizeX = Mathf.FloorToInt(objectSize.x / 2);
        int halfSizeY = Mathf.FloorToInt(objectSize.y / 2);

        for (int x = -halfSizeX; x <= halfSizeX; x++)
        {
            for (int z = -halfSizeY; z <= halfSizeY; z++)
            {
                positions.Add(gridPosition + new Vector3Int(x, 0, z));
            }
        }

        return positions;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2 objectSize)
    {
        List<Vector3Int> positionsToCheck = CalculatePositions(gridPosition, objectSize);
        foreach (var position in positionsToCheck)
        {
            if (placedObjectsData.ContainsKey(position))
            {
                return false;
            }
        }
        return true;
    }

    internal int GetObjectIndexAt(Vector3Int gridPosition)
    {
        if (placedObjectsData.ContainsKey(gridPosition))
        {
            return placedObjectsData[gridPosition].PlacedObjectIndex;
        }
        return -1;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var position in placedObjectsData[gridPosition].occupiedCells)
        {
            placedObjectsData.Remove(position);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedCells;

    public int ID { get; private set; }

    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedCells, int iD, int placedObjectIndex)
    {
        this.occupiedCells = occupiedCells;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}