using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerBuildMode : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Camera mainCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayer;

    [SerializeField]
    private PlacementSystem placementSystem;

    public event Action OnClicked, OnExit;

    #endregion

    void OnEnable()
    {
        placementSystem.BuildMode(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
            UIManager.Instance.BuildModeButton();
        }
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    #region BuildMode Methods

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayer))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    #endregion

    void OnDisable()
    {
        placementSystem.BuildMode(false);
    }
}
