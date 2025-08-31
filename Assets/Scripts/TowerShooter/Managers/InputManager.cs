using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private Camera mainCamera;
    private bool isBuildMode = false;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayer;

    [SerializeField]
    private PlacementSystem placementSystem;

    #endregion

    #region Unity Methods

    void Update()
    {
        if (!isBuildMode)
        {
            WaveMode();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.StartTheGame();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildMode = !isBuildMode;
            placementSystem.BuildMode(isBuildMode);
        }
    }

    #endregion

    #region WaveMode Methods

    private void WaveMode()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.upgradePanelOpen)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    GameManager.Instance.AddScore(hit.collider.gameObject.GetComponent<Enemy>()._enemyValue);
                    hit.collider.gameObject.GetComponent<Enemy>().GotKilled();
                }
            }
        }
    }

    #endregion

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

    public bool BuildMode()
    {
        isBuildMode = !isBuildMode;
        placementSystem.BuildMode(isBuildMode);
        return isBuildMode;
    }

    #endregion
}
