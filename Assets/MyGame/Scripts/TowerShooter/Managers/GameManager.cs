using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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

    public int _savedDayCount { get; private set; }
    public int _currentDayCount { get; private set; }
    public int _enemyCount { get; private set; }
    public int _playerScore { get; private set; }
    public int _playerCombo { get; private set; }
    public int saveIndex { get; private set;} = 0;
    public bool isBuildMode { get; private set; } = false;
    public bool _canSkip { get; private set; } = true;
    public bool canSpawnNextWave, willEnemySpawn;
    public SpawnManager spawnManager;
    [SerializeField]
    private GameObject inputManager, inputManagerBuildMode, _playerTower;
    public GridDataSaver gridDataSaver;
    public InventoryManager inventoryManager;
    public ShopManager shopManager;
    public Light globalLight,
                 towerLight;

    private float protectionTime;
    private float currentTime;


    public bool gamePaused = false;

    #endregion

    #region Unity Methods

    void Start()
    {
        InitializeSavedData();
        _enemyCount = 0;
        _playerCombo = 1;
        canSpawnNextWave = false;
        willEnemySpawn = false;
        protectionTime = 60;
        currentTime = 0;
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.UpdateWaveBar(0, true);
        UIManager.Instance.WaveStatus(false);
        inputManagerBuildMode.GetComponent<InputManagerBuildMode>().enabled = false;
        inputManager.GetComponent<InputManager>().enabled = true;
        StartCoroutine(ProtectionDecay());
        UnlockPurchaseItemsDayCheck();
    }

    void Update()
    {
        if(!IsWaveGoing()) UIManager.Instance.UpdateWaveBar(1-(currentTime / protectionTime), true);
        //GlobalLightDayCycle(currentTime / dayCycleTime);
        if(!gamePaused) currentTime += Time.deltaTime;
    }
    #endregion

    #region GameRelated Change Methods

    public void WaveOver()
    {
        StopAllCoroutines();
        _enemyCount = 0;
        currentTime = 0;
        spawnManager.WaveOver();
        _savedDayCount++;
        UnlockPurchaseItemsDayCheck();
        GameObject.Find("PlayerTower").GetComponent<PlayerTower>().GameOver();
        KillAllAvailableEnemies();
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.WaveStatus(false);
    }

    public void GameOver()
    {
        GamePaused(true);
        UIManager.Instance.GameOver();
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
            UIManager.Instance.ShowTheTurretDetails(false);
            ModeChanged();
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
        AudioManager.Instance.PlayTheAudioClip(AudioType.EnemyDestroyed);
        if (_enemyCount == 0 && !willEnemySpawn)
        {
            AudioManager.Instance.PlayTheAudioClip(AudioType.WaveCleared);
            currentTime = 0;
            WaveCleared();
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
            Debug.Log("Purchased Item for " + value);
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
        ComboChecker();
        _playerScore += score*_playerCombo;
        UIManager.Instance.UpdateUIElements();
    }

    public void UnlockPurchaseItemsDayCheck()
    {
        if(_savedDayCount>=5) shopManager.UnlockTurretPurchaseItem(0);
        if(_savedDayCount>=10) shopManager.UnlockTurretPurchaseItem(1);
        if(_savedDayCount>=15) shopManager.UnlockTurretPurchaseItem(2);
        if(_savedDayCount>=20) shopManager.UnlockTurretPurchaseItem(3);
    }

    #endregion

    #region Coroutines

    IEnumerator ProtectionDecay()
    {
        yield return new WaitForSeconds(protectionTime);
        ProtectionEnded();
    }

    #endregion

    public bool IsWaveGoing()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Count() > 0 ? true : false;
    }

    public void StartTheWave()
    {
        UIManager.Instance.WaveStatus(true);
        if(shopManager.ShopStatus()) shopManager.ShopPanelActivate();
        _currentDayCount++;
        UIManager.Instance.UpdateUIElements();
        UIManager.Instance.ShowTheTurretDetails(false);
        ToolTipManager.Instance.Hide();
        spawnManager.StartTheSpawn(_currentDayCount);
        ModeChanged();
    }

    private void ProtectionEnded()
    {
        UIManager.Instance.WaveStatus(true);
        UIManager.Instance.UpdateWaveBar(0, true);
        if(isBuildMode) UIManager.Instance.BuildModeButton();
        StartTheWave();
    }

    public void WaveCleared()
    {
        UIManager.Instance.WaveStatus(false);
        _savedDayCount = _currentDayCount;
        UIManager.Instance.UpdateWaveBar(1, true);
        PlayerDataSaver.SavePlayerData(NeedAllDataToSave());
        StartCoroutine(ProtectionDecay());
        UnlockPurchaseItemsDayCheck();
    }

    public void SkipTheProtection()
    {
        StopCoroutine(ProtectionDecay());
        ProtectionEnded();
    }

    private void KillAllAvailableEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var ene in enemies)
        {
            if(ene.TryGetComponent(out Enemy e))
            {
                EnemyPool.Instance.ReturnToPool(e);
            }
        }
    }

    public void IncreaseDayCount()
    {
        _currentDayCount++;
        _savedDayCount = _currentDayCount;
        PlayerDataSaver.SavePlayerData(NeedAllDataToSave());
        UIManager.Instance.UpdateUIElements();
        UnlockPurchaseItemsDayCheck();

    }

    public UnityEvent onModeChange;

    void ModeChanged()
    {
        onModeChange?.Invoke();
    }

    private float   lastComboTime = 0;
    [SerializeField]
    public float _comboInterval { get; private set; }  = 1f;

    public void ComboChecker()
    {
        if(lastComboTime + _comboInterval < Time.time)
        {
            lastComboTime = Time.time;
            _playerCombo = 1;
            UIManager.Instance.AddToCombo(_playerCombo);
        }
        else
        {
            lastComboTime = Time.time;
            _playerCombo++;
            UIManager.Instance.AddToCombo(_playerCombo);
        }
    }

    public void SaveTheGridData(PlacementData placementData)
    {
        gridDataSaver.SaveGridData(placementData);
    }

    public void RemoveObjectFromData(int gameObjectIndex)
    {
        gridDataSaver.RemoveObjectFromSavedData(gameObjectIndex);
    }

    public void ClearSavedGridData()
    {
        gridDataSaver.ClearSavedData();
    }

    public void LoadTheGridData()
    {
        gridDataSaver.LoadTheGridData();
    }

    public void SaveCurrentPlayerData()
    {
        PlayerDataSaver.SavePlayerData(NeedAllDataToSave());
    }

    public PlayerData NeedAllDataToSave()
    {
        return new PlayerData(
            GameObject.Find("PlayerTower").GetComponent<PlayerTower>().GetTowerHealth(),
            _playerScore,
            _savedDayCount,
            inventoryManager.GetAllItems()
        );
    }


    public void InitializeSavedData()
    {
        PlayerData data = PlayerDataSaver.LoadPlayerData();
        if (data == null)
        {
            _playerScore = 0;
            _currentDayCount = 0;
            _savedDayCount = 0;
            _playerTower.GetComponent<PlayerTower>().SetMaxTowerHealth();
            inventoryManager.LoadItems(new Dictionary<int, int>());
            PlayerDataSaver.SavePlayerData(NeedAllDataToSave());
            return;
        }
        _playerScore = data.playerScore;
        _currentDayCount = data.waveNumber;
        _savedDayCount = data.waveNumber;
        _playerTower.GetComponent<PlayerTower>().SetTowerHealth(data.towerHealth);
        inventoryManager.LoadItems(data.inventoryItems);
        if(data.towerHealth <= 0) GameOver();
    }

    public void ReduceInventory()
    {
        inventoryManager.RemoveItem(0, 1);
    }

    public void ReduceScoreToZero()
    {
        _playerScore = 0;
        UIManager.Instance.UpdateUIElements();
    }

    public void OpenShop()
    {
        shopManager.ShopPanelActivate();
    }

    public TurretData GetTurretData(int turretID)
    {
        TurretSaveData allTurretData = shopManager.GetCurrentTurretData();
        if(allTurretData == null)
        {
            return null;
        }
        return allTurretData.turrets[turretID];
    }

    public bool MenuPanelStatus()
    {
        return shopManager.ShopStatus();
    }

    public void RegenTowerHealth()
    {
        _playerTower.GetComponent<PlayerTower>().SetMaxTowerHealth();
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
            Debug.Log("Application quit");
        #else
            Application.Quit();
        #endif
    }

    public void MainMenu()
    {
        SaveCurrentPlayerData();
        SceneManager.LoadScene(0);
    }

    public void SaveTheGame()
    {
        SaveCurrentPlayerData();
    }

    public void GamePaused(bool status)
    {
        Time.timeScale = status ? 0 : 1;
    }
}

