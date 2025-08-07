using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is Null");
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }


    private int _waveLevel;
    private int _enemyCount;

    void Start()
    {
        _waveLevel = 0;
        _enemyCount = 0;
        UIManager.Instance.UpdateUIElements();
    }

    public void PauseTheGame()
    {
        Debug.Log("Game Paused");
    }

    public void EnemyGotDestroyed()
    {
        _enemyCount--;
        UIManager.Instance.UpdateUIElements();
    }

    public void EnemyGotSpawned()
    {
        _enemyCount++;
        UIManager.Instance.UpdateUIElements();
    }

    public int GetEnemyCount()
    {
        return _enemyCount;
    }
}
