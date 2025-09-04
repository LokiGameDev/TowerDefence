using System;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData gridData;
    ObjectPlacer objectPlacer;

    public PlacementState(int ID, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData gridData, ObjectPlacer objectPlacer)
    {
        this.ID = ID;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(x => x.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPreview(database.objectsData[selectedObjectIndex].prefab, database.objectsData[selectedObjectIndex].size);
        }
        else
        {
            throw new Exception($"{ID} not found in database.");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool canPlace = CheckForPlacementValidity(gridPosition, selectedObjectIndex);

        if (!canPlace) return;

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPosition));

        gridData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].size, database.objectsData[selectedObjectIndex].ID, index);
        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckForPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].size);
    }
    
    public void UpdateState(Vector3Int gridPosition)
    {
        bool canPlace = CheckForPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
    }   
}
