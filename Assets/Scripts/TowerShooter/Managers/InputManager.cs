using System.Collections;
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
        if (GameManager.Instance._canSkip)
        {
            if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(StartingAction());
            if (Input.GetKeyUp(KeyCode.Space)) StopCoroutine(StartingAction());
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
                    hit.collider.gameObject.GetComponent<IDamagable>().GotHit();
                }
            }
        }
    }

    #endregion

    IEnumerator StartingAction()
    {
        yield return new WaitForSeconds(3);
    }

}
