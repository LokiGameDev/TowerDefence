using UnityEngine;
using UnityEngine.SceneManagement;

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


    public int _waveLevel { get; private set; }
    public int _enemyCount { get; private set; }
    public int _playerScore { get; private set; }
    public SpawnManager spawnManager;

    void Start()
    {
        _waveLevel = 1;
        _enemyCount = 0;
        _playerScore = 0;
        spawnManager.StartTheSpawn(_waveLevel);
        UIManager.Instance.UpdateUIElements();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        UIManager.Instance.GameOver();
    }

    public void EnemyGotDestroyed(bool gotKilled)
    {
        _enemyCount--;
        if (gotKilled) _playerScore++;
        UIManager.Instance.UpdateUIElements();
    }

    public void EnemyGotSpawned()
    {
        _enemyCount++;
        UIManager.Instance.UpdateUIElements();
    }

    public void SpawnTheNextWave()
    {
        if (_enemyCount <= 0)
        {
            _waveLevel++;
            spawnManager.StartTheSpawn(_waveLevel);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
