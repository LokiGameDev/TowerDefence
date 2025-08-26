using System.Collections;
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
    private float spawnWaveWaitTime;
    private float currentTime;
    private bool spawnWaveInterval;
    public SpawnManager spawnManager;
    public bool _attackAbility { get; private set; }

    void Start()
    {
        _waveLevel = 1;
        _enemyCount = 0;
        _playerScore = 0;
        spawnWaveWaitTime = 10;
        spawnWaveInterval = false;
        spawnManager.StartTheSpawn(_waveLevel);
        _attackAbility = false;
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.UpdateWaveBar(false);
    }

    void Update()
    {
        if (spawnWaveInterval)
        {
            UIManager.Instance.UpdateWaveBar(currentTime / spawnWaveWaitTime);
            currentTime += Time.deltaTime;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        UIManager.Instance.GameOver();
    }

    public void EnemyGotDestroyed(bool gotKilled)
    {
        _enemyCount--;
        if (_enemyCount <= 0)
        {
            currentTime = 0;
            StartCoroutine(SpawnWaveIntervalTime());
        }
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

    IEnumerator SpawnWaveIntervalTime()
    {
        spawnWaveInterval = true;
        UIManager.Instance.UpdateWaveBar(true);
        yield return new WaitForSeconds(spawnWaveWaitTime);
        spawnWaveInterval = false;
        UIManager.Instance.UpdateWaveBar(false);
        SpawnTheNextWave();
    }

    public void GamePauseStatus(bool status)
    {
        Time.timeScale = status ? 1 : 0;
    }

    public void Purchasing(int value)
    {
        _playerScore -= value;
        UIManager.Instance.UpdateUIElements();
    }

    public void UnlockAttackAbility()
    {
        _attackAbility = true;
    }

    public void AddScore(int score)
    {
        _playerScore += score;
        UIManager.Instance.UpdateUIElements();
    }
}
