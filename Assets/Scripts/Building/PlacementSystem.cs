using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject gridLineVisual;
    [SerializeField]
    private InputManagerBuildMode inputManagerBuildMode;
    public Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    private bool isBuildMode = false;

    public GridData gridData;

    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastValidPosition = Vector3Int.zero;
    public ObjectPlacer objectPlacer;

    [SerializeField]
    private InventoryManager inventoryManager;

    IBuildingState buildingState;

    #endregion

    void Start()
    {
        StopPlacement();
        gridData = new GridData();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridLineVisual.SetActive(true);
        buildingState = new PlacementState(ID, grid, previewSystem, database, gridData, objectPlacer, inventoryManager);
        inputManagerBuildMode.OnClicked += PlaceStructure;
        inputManagerBuildMode.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridLineVisual.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, gridData, objectPlacer);
        inputManagerBuildMode.OnClicked += PlaceStructure;
        inputManagerBuildMode.OnExit += StopPlacement;  
    }

    private void PlaceStructure()
    {
        if (inputManagerBuildMode.IsPointerOverUI()) return;
        Vector3 mousePosition = inputManagerBuildMode.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        if (buildingState == null) return;
        gridLineVisual.SetActive(false);
        buildingState.EndState();
        inputManagerBuildMode.OnClicked -= PlaceStructure;
        inputManagerBuildMode.OnExit -= StopPlacement;
        lastValidPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if (!isBuildMode || buildingState == null) return;

        Vector3 mousePosition = inputManagerBuildMode.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        if (lastValidPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastValidPosition = gridPosition;
        }
    }

    public void BuildMode(bool state)
    {
        if(gridLineVisual!=null) gridLineVisual.SetActive(state);
        isBuildMode = state;
    }
}
