using UnityEngine;
using System.Collections;
using System;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    public bool _canSpawnEnemy;
    private int _enemyCount;
    private float _spawnRadius;

    #endregion

    #region Unity Methods

    void Start()
    {
        _canSpawnEnemy = true;
        _spawnRadius = 35;
    }

    #endregion

    #region Spawn Methods

    public void StartTheSpawn(int level)
    {
        GameManager.Instance.willEnemySpawn = true;
        _enemyCount = level*2;
        SpawnWave();
    }

    void SpawnWave()
    {
        if (_canSpawnEnemy)
        {
            var enemy = EnemyPool.Instance.Get();
            GameManager.Instance.EnemyGotSpawned();
            enemy.gameObject.transform.position = GenerateRandomSpawnLoc();
            _enemyCount--;
            enemy.gameObject.SetActive(true);
            _canSpawnEnemy = false;
            StartCoroutine(SpawnTimeInterval());
        }
    }

    #endregion

    #region Help Methods

    public Vector3 GenerateRandomSpawnLoc()
    {
        Vector2 circle = UnityEngine.Random.insideUnitCircle.normalized;
        float radius = _spawnRadius + UnityEngine.Random.Range(0f, 10f);

        return new Vector3(circle.x * radius, 0.5f, circle.y * radius);
    }

    public void WaveOver()
    {
        _enemyCount = 0;
        GameManager.Instance.willEnemySpawn = false;
        StopAllCoroutines();
    }

    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        Vector3 center = transform.position;

        DrawCircle(center, _spawnRadius, Color.red);
        DrawCircle(center, _spawnRadius+10, Color.yellow);
    }

    void DrawCircle(Vector3 center, float radius, Color color)
    {
        Gizmos.color = color;

        const int segments = 64;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 nextPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    #endregion

    #region Coroutines

    IEnumerator SpawnTimeInterval()
    {
        if (_enemyCount <= 0)
        {
            GameManager.Instance.willEnemySpawn = false;
        }
        yield return new WaitForSeconds(1);
        _canSpawnEnemy = true;
        if (_enemyCount > 0)
        {
            SpawnWave();
        }
    }

    #endregion
}
