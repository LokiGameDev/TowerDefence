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
    private float dayCycleTime;
    private float currentTime;
    public bool isBuildMode { get; private set; } = false;
    public bool _canSkip { get; private set; } = true;
    public bool canSpawnNextWave, willEnemySpawn;
    public SpawnManager spawnManager;
    [SerializeField]
    private GameObject inputManager, inputManagerBuildMode;

    enum DayCycle
    {
        DayTime,
        NightTime,
    }

    private DayCycle currentDayCycle;

    #endregion

    #region Unity Methods

    void Start()
    {
        _waveLevel = 0;
        _enemyCount = 0;
        _playerScore = 0;
        dayCycleTime = 30;
        canSpawnNextWave = false;
        willEnemySpawn = false;
        currentDayCycle = DayCycle.DayTime;
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.UpdateWaveBar(0, false);
        UIManager.Instance.WaveStatus(false);
        inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = false;
        inputManager.GetComponent<InputManager>().enabled = true;
        StartCoroutine(DayNightCycle());
    }

    void Update()
    {
        UIManager.Instance.UpdateWaveBar(currentTime / dayCycleTime, true);
        currentTime += Time.deltaTime;
    }
    #endregion

    #region GameRelated Change Methods

    public void WaveOver()
    {
        StopAllCoroutines();
        _enemyCount = 0;
        currentTime = 0;
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
            UIManager.Instance.BuildModeStatus(true);
        }
        else
        {
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().OnExitMethod();
            inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = false;
            inputManager.GetComponent<InputManager>().enabled = true;
            UIManager.Instance.BuildModeStatus(true);
        }
    }

    #endregion

    #region GameValue Change Methods

    public void EnemyGotDestroyed()
    {
        _enemyCount--;
        if (_enemyCount == 0 && !willEnemySpawn) UIManager.Instance.AllEnemiesAreCleared();
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

    IEnumerator DayNightCycle()
    {
        currentTime = 0;
        yield return new WaitForSeconds(dayCycleTime);
        DayEnded();
    }


    #endregion

    public bool IsWaveGoing()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Count() > 0 ? true : false;
    }

    public void StartTheWave()
    {
        UIManager.Instance.WaveStatus(true);
        _waveLevel++;
        UIManager.Instance.UpdateUIElements();
        spawnManager.StartTheSpawn(_waveLevel);
    }

    private void DayEnded()
    {
        StopCoroutine(DayNightCycle());
        currentDayCycle = currentDayCycle == DayCycle.DayTime ? DayCycle.NightTime : DayCycle.DayTime;
        Debug.Log(currentDayCycle);
        if (currentDayCycle == DayCycle.NightTime)
        {
            UIManager.Instance.WaveStatus(true);
            if(isBuildMode) UIManager.Instance.BuildModeButton();
            StartTheWave();
        }
        else
        {
            WaveOver();
        }
        StartCoroutine(DayNightCycle());
    }

    public void WaveCleared()
    {
        Debug.Log("Wave been cleared");
    }

    public void SkipTheDayNight()
    {
        StopCoroutine(DayNightCycle());
        DayEnded();
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