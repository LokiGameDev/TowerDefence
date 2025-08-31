using UnityEngine;
using System.Collections;
using System;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    private bool _canSpawnEnemy;
    private int _waveLevel;
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
        _waveLevel = level;
        _waveLevel--;
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

    #endregion
    
    #region Coroutines

    IEnumerator SpawnTimeInterval()
    {
        yield return new WaitForSeconds(1);
        _canSpawnEnemy = true;
        if (_waveLevel > 0)
        {
            _waveLevel--;
            SpawnWave();
        }
    }

    #endregion
}
