using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private InventoryManager inventoryManager;

    #endregion

    #region Unity Methods

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryManager?.AddItem(0, 1);
        }
        WaveMode();
    }

    #endregion

    #region WaveMode Methods

    private void WaveMode()
    {
        if (Input.GetMouseButtonDown(0))
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

}
