using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject placementIndicator, buildingPositionIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid; 

    private bool isBuildMode = false;

    #endregion

    void Start()
    {
        placementIndicator.SetActive(false);
        buildingPositionIndicator.SetActive(false);
    }

    private void Update()
    {
        if (!isBuildMode) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        placementIndicator.transform.position = mousePosition;
        buildingPositionIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void BuildMode(bool state)
    {
        placementIndicator.SetActive(state);
        buildingPositionIndicator.SetActive(state);
        isBuildMode = state;
    }
}
