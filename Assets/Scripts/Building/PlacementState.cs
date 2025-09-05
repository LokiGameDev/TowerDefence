using System;
using UnityEngine;

public class PlacementState : IBuildingState
{
    #region Variables

    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData gridData;
    ObjectPlacer objectPlacer;
    InventoryManager inventoryManager;

    #endregion

    #region Events

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
        inventoryManager.RemoveItem(ID, 1);
        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool canPlace = CheckForPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
    }

    #endregion

    public PlacementState(int ID, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData gridData, ObjectPlacer objectPlacer, InventoryManager inventoryManager)
    {
        this.ID = ID;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.inventoryManager = inventoryManager;

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

    private bool CheckForPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].size) && inventoryManager.GetItemCount(ID) > 0;
    } 
}
