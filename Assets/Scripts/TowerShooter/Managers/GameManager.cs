using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
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
    #endregion

    #region Variables

    public int _waveLevel { get; private set; }
    public int _enemyCount { get; private set; }
    public int _playerScore { get; private set; }
    private float spawnWaveWaitTime;
    private float currentTime;
    private bool spawnWaveInterval;
    public bool isBuildMode { get; private set; } = false;
    public bool _attackAbility { get; private set; }
    public SpawnManager spawnManager;
    [SerializeField]
    private GameObject inputManager, inputManagerBuildMode;

    #endregion

    #region Unity Methods

    void Start()
    {
        _waveLevel = 1;
        _enemyCount = 0;
        _playerScore = 0;
        spawnWaveWaitTime = 10;
        spawnWaveInterval = false;
        _attackAbility = false;
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.UpdateWaveBar(0, false);
        inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = false;
        inputManager.GetComponent<InputManager>().enabled = true;
    }

    void Update()
    {
        if (spawnWaveInterval)
        {
            UIManager.Instance.UpdateWaveBar(currentTime / spawnWaveWaitTime, true);
            currentTime += Time.deltaTime;
        }
    }
    #endregion

    #region GameRelated Change Methods

    public void GameOver()
    {
        Debug.Log("Game Over");
        UIManager.Instance.GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SpawnTheNextWave()
    {
        if (_enemyCount <= 0)
        {
            _waveLevel++;
            spawnManager.StartTheSpawn(_waveLevel);
        }
    }

    public void GamePauseStatus(bool status)
    {
        Time.timeScale = status ? 1 : 0;
    }

    public void StartTheGame()
    {
        spawnManager.StartTheSpawn(_waveLevel);
    }

    public void BuildMode()
    {
        isBuildMode = !isBuildMode;
        if (isBuildMode)
        {
            Time.timeScale = 0;
            inputManager.GetComponent<InputManager>().enabled = false;
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = false;
            inputManager.GetComponent<InputManager>().enabled = true;
        }
    }

    #endregion

    #region GameValue Change Methods

    public void EnemyGotDestroyed()
    {
        _enemyCount--;
        if (_enemyCount <= 0)
        {
            currentTime = 0;
            StartCoroutine(SpawnWaveIntervalTime());
        }
    }

    public void EnemyGotSpawned()
    {
        _enemyCount++;
        UIManager.Instance.UpdateUIElements();
    }

    public void Purchasing(int value)
    {
        _playerScore -= value;
        UIManager.Instance.UpdateUIElements();
    }

    public void UnlockAttackAbility()
    {
        _attackAbility = true;
        UIManager.Instance.AbilityUnlock();
    }

    public void AddScore(int score)
    {
        _playerScore += score;
        UIManager.Instance.UpdateUIElements();
    }

    #endregion

    #region Coroutines

    IEnumerator SpawnWaveIntervalTime()
    {
        spawnWaveInterval = true;
        UIManager.Instance.UpdateWaveBar(0, true);
        yield return new WaitForSeconds(spawnWaveWaitTime);
        spawnWaveInterval = false;
        UIManager.Instance.UpdateWaveBar(0, false);
        SpawnTheNextWave();
    }

    #endregion
}