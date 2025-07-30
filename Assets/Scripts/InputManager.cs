using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;

    private bool _canSpawnEnemy;

    void Start()
    {
        _canSpawnEnemy = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    GameManager.Instance.EnemyGotDestroyed();
                    hit.collider.gameObject.GetComponent<Enemy>().GotKilled();

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && _canSpawnEnemy)
        {
            var enemy = EnemyPool.Instance.Get();
            enemy.gameObject.transform.position = new Vector3(10, 0.5f, 0);
            enemy.gameObject.SetActive(true);
            _canSpawnEnemy = false;
            StartCoroutine(SpawnTimeInterval());
        }
    }

    IEnumerator SpawnTimeInterval()
    {
        yield return new WaitForSeconds(1);
        _canSpawnEnemy = true;
    }
}
