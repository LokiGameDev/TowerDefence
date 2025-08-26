using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField]
    private Enemy enemyPrefab;
    [SerializeField]
    private Queue<Enemy> enemyPool = new Queue<Enemy>();

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
    void Awake()
    {
        _instance = this;
    }

    public Enemy Get()
    {
        if (enemyPool.Count == 0)
        {
            AddEnemy(1);
        }
        return enemyPool.Dequeue();
    }

    private void AddEnemy(int coount)
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.gameObject.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    public void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
