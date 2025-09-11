using System.Collections;
using System.Linq;
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
    public bool _isWaveStarted { get; private set; } = false;
    public bool canSpawnNextWave, willEnemySpawn;
    public SpawnManager spawnManager;
    [SerializeField]
    private GameObject inputManager, inputManagerBuildMode;

    #endregion

    #region Unity Methods

    void Start()
    {
        _waveLevel = 10;
        _enemyCount = 0;
        _playerScore = 0;
        spawnWaveWaitTime = 10;
        spawnWaveInterval = false;
        canSpawnNextWave = false;
        willEnemySpawn = false;
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.UpdateWaveBar(0, false);
        UIManager.Instance.WaveStatus(false);
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

    public void WaveOver()
    {
        StopAllCoroutines();
        _isWaveStarted = false;
        _waveLevel = 0;
        _enemyCount = 0;
        currentTime = 0;
        spawnWaveInterval = false;
        UIManager.Instance.UpdateWaveBar(0, false);
        spawnManager.WaveOver();
        GameObject.Find("PlayerTower").GetComponent<PlayerTower>().GameOver();
        KillAllAvailableEnemies();
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.WaveStatus(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void BuildMode()
    {
        isBuildMode = !isBuildMode;
        if (isBuildMode)
        {
            inputManager.GetComponent<InputManager>().enabled = false;
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = true;
        }
        else
        {
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().OnExitMethod();
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = false;
            inputManager.GetComponent<InputManager>().enabled = true;
        }
    }

    #endregion

    #region GameValue Change Methods

    public void EnemyGotDestroyed()
    {
        _enemyCount--;
        if (_enemyCount <= 0 && !willEnemySpawn)
        {
            currentTime = 0;
            StartCoroutine(SpawnWaveIntervalTime());
        }
        UIManager.Instance.UpdateUIElements();
    }

    public void EnemyGotSpawned()
    {
        _enemyCount++;
        UIManager.Instance.UpdateUIElements();
    }

    public bool Purchasing(int value)
    {
        if (_playerScore >= value)
        {
            _playerScore -= value;
            UIManager.Instance.UpdateUIElements();
            return true;
        }
        else
        {
            UIManager.Instance.DisplayInformation("Not enough Money");
        }
        UIManager.Instance.UpdateUIElements();
        return false;
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
        canSpawnNextWave = true;
        _isWaveStarted = false;
        StartTheWave();
    }


    #endregion

    public bool IsWaveGoing()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Count() > 0 ? true : false;
    }

    public void StartTheWave()
    {
        if (!_isWaveStarted)
        {
            UIManager.Instance.WaveStatus(true);
            _waveLevel++;
            spawnManager.StartTheSpawn(_waveLevel);
            _isWaveStarted = true;
        }
    }

    private void KillAllAvailableEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var ene in enemies)
        {
            EnemyPool.Instance.ReturnToPool(ene.GetComponent<Enemy>());
        }
    }
}