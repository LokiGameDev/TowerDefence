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
        _spawnRadius = 20;
    }

    #endregion

    #region Spawn Methods

    public void StartTheSpawn(int level)
    {
        GameManager.Instance.willEnemySpawn = true;
        _enemyCount = level - 1;
        SpawnWave();
    }

    void SpawnWave()
    {
        if (_canSpawnEnemy)
        {
            var enemy = EnemyPool.Instance.Get();
            GameManager.Instance.EnemyGotSpawned();
            enemy.gameObject.transform.position = GenerateRandomSpawnLoc();
            enemy.gameObject.SetActive(true);
            _canSpawnEnemy = false;
            StartCoroutine(SpawnTimeInterval());
        }
    }

    #endregion

    #region Help Methods

    public Vector3 GenerateRandomSpawnLoc()
    {
        int theta = UnityEngine.Random.Range(0, 360);

        float offset = UnityEngine.Random.Range(0, 10);

        float z = (_spawnRadius + offset) * (float)Math.Sin(theta);
        float x = (_spawnRadius + offset) * (float)Math.Cos(theta);

        Vector3 pos = new Vector3(x, 0.5f, z);

        return pos;
    }

    public void WaveOver()
    {
        _enemyCount = 0;
        StopAllCoroutines();
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
            _enemyCount--;
            SpawnWave();
        }
    }

    #endregion
}
