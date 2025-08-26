using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.upgradePanelOpen)
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().GotKilled();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.AddScore(10);
        }
    }
}
