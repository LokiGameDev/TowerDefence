using System;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject placementIndicator, buildingPositionIndicator, gridLineVisual;
    [SerializeField]
    private InputManagerBuildMode inputManagerBuildMode;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    private bool isBuildMode = false;

    #endregion

    void Start()
    {
        placementIndicator.SetActive(false);
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(x => x.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"{ID} not found in database.");
            return;
        }
        buildingPositionIndicator.SetActive(true);
        gridLineVisual.SetActive(true);
        inputManagerBuildMode.OnClicked += PlaceStructure;
        inputManagerBuildMode.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManagerBuildMode.IsPointerOverUI()) return;
        Vector3 mousePosition = inputManagerBuildMode.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject playerTurret = Instantiate(database.objectsData[selectedObjectIndex].prefab);
        playerTurret.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        buildingPositionIndicator.SetActive(false);
        gridLineVisual.SetActive(false);
        inputManagerBuildMode.OnClicked -= PlaceStructure;
        inputManagerBuildMode.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (!isBuildMode || selectedObjectIndex < 0) return;

        Vector3 mousePosition = inputManagerBuildMode.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        placementIndicator.transform.position = mousePosition;
        buildingPositionIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void BuildMode(bool state)
    {
        placementIndicator.SetActive(state);
        buildingPositionIndicator.SetActive(state);
        gridLineVisual.SetActive(state);
        isBuildMode = state;
    }
}
