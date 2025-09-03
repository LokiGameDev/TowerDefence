using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private Camera mainCamera;
    private bool gameStarted = false;

    #endregion

    #region Unity Methods

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && !GameManager.Instance.isBuildMode && !gameStarted)
        {
            GameManager.Instance.StartTheGame();
            gameStarted = true;
        }
        WaveMode();
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

}
