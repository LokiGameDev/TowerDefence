using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private Enemy[] enemyPrefabs;
    [SerializeField]
    private Queue<Enemy> enemyPool = new Queue<Enemy>();

    #endregion

    #region Singleton

    private static EnemyPool _instance;
    public static EnemyPool Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("EnemyPool is null");
            }
            return _instance;
        }
    }
    void Awake() => _instance = this;

    #endregion

    #region Functions

    public Enemy Get()
    {
        if (enemyPool.Count == 0)
        {
            AddEnemy(1);
        }
        return enemyPool.Dequeue();
    }

    private void AddEnemy(int count)
    {
        Enemy enemy = Instantiate(SpawnRandomEnemy());
        enemy.gameObject.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    public void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    public Enemy SpawnRandomEnemy()
    {
        int val = GameManager.Instance._currentDayCount;
        int index = Random.Range(0,val>=20 ? 3 : val>=15 ? 2 : val>=10 ? 1 : 0);
        return enemyPrefabs[index];
    }

    #endregion
}
