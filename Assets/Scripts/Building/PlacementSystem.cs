using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject placementIndicator;
    [SerializeField]
    private InputManager inputManager;
    private bool isBuildMode = false;

    #endregion

    private void Update()
    {
        if (!isBuildMode) return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        placementIndicator.transform.position = mousePosition;
    }

    public void BuildMode(bool state)
    {
        placementIndicator.SetActive(state);
        isBuildMode = state;
    }
}
