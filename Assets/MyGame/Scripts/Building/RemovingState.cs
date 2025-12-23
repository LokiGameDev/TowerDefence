using UnityEngine;

public class RemovingState : IBuildingState
{
    #region Variables

    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData gridData;
    ObjectPlacer objectPlacer;

    #endregion

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData gridData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    #region Events

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = gridData;
        if (selectedData == null) return;
        else
        {
            gameObjectIndex = selectedData.GetObjectIndexAt(gridPosition);
            if (gameObjectIndex < 0) return;
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObject(gameObjectIndex);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePreviewPosition(cellPosition, !selectedData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool canRemove = !gridData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canRemove);
    }
    
    #endregion
}
