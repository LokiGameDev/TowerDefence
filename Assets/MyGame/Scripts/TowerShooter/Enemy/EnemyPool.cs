using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Enemy[] enemyPrefabs;
    private Dictionary<EnemyType, Queue<Enemy>> enemyTypePools = new Dictionary<EnemyType, Queue<Enemy>>();
    [SerializeField]
    private GameObject enemyParent;

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
        int index = Random.Range(0, (GameManager.Instance._currentDayCount/5)+1);
        EnemyType enemyType = (EnemyType)Mathf.Clamp(index, 0, System.Enum.GetValues(typeof(EnemyType)).Length - 1);
        return GetByType(enemyType);
    }

    public Enemy GetByType(EnemyType type)
    {
        if(!enemyTypePools.ContainsKey(type))
        {
            enemyTypePools[type] = new Queue<Enemy>();
        }
        if (enemyTypePools[type].Count == 0)
        {
            Enemy enemy = Instantiate(SpawnEnemy(type),enemyParent.transform);
            enemy.gameObject.SetActive(false);
            enemyTypePools[type].Enqueue(enemy);
        }
        return enemyTypePools[type].Dequeue();
    }

    public void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemyTypePools[enemy.enemyType].Enqueue(enemy);
    }

    private Enemy SpawnEnemy(EnemyType type)
    {
        foreach (var enemy in enemyPrefabs)
        {
            if (enemy.enemyType == type)
            {
                return enemy;
            }
        }
        Debug.LogError($"Enemy of type {type} not found!");
        return null;
    }

    #endregion
}


public enum EnemyType
{
    LightBug,
    NoctisWasp,
    SolarbaneMoth,
    Gloomcrawler
}
