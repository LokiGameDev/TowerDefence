using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is null");
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
    public Text enemyCount,
                playerScore,
                waveLevel;
    public Image towerHealthBar,
                 waveBar;
    public GameObject gameOverPanel,
                      towerUpgradePanel,
                      turretPurchasePanelGameObject,
                      buildModePanel;
    
    public GameObject[] abilityLock;
    public bool upgradePanelOpen;
    [SerializeField]
    private PlayerTower playerTower;
    [SerializeField]
    private InventoryManager inventoryManager;
    public Text[] inventoryTexts;

    #endregion

    #region Unity Methods

    void Start()
    {
        gameOverPanel.SetActive(false);
        towerUpgradePanel.SetActive(false);
        upgradePanelOpen = false;
        foreach(var abilityLockObj in abilityLock)
        {
            abilityLockObj.SetActive(true);
        }
        turretPurchasePanelGameObject.SetActive(false);
        buildModePanel.SetActive(false);
    }
    #endregion

    #region Button Methods

    public void RestartButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void UpgradeButton()
    {
        TowerUpgradePanel(!towerUpgradePanel.activeSelf);
    }

    public void BuildModeButton()
    {
        GameManager.Instance.BuildMode();
        buildModePanel.SetActive(GameManager.Instance.isBuildMode);
    }

    #endregion

    #region Tower Upgrade Methods

    public void TowerUpgradePanel(bool status)
    {
        towerUpgradePanel.SetActive(status);
        if (!status) turretPurchasePanelGameObject.SetActive(status);
        upgradePanelOpen = status;
        GameManager.Instance.GamePauseStatus(!status);
    }

    public void TowerUpgradeButtons(int ID)
    {
        switch (ID)
        {
            case 0:
                playerTower.TowerHealthUpgrade();
                break;
            case 1:
                playerTower.TowerAttackSpeedUpgrade();
                break;
            case 2:
                playerTower.TowerTurretUpgrade();
                break;
            default:
                break;
        }
    }

    public void AbilityUnlock(int index)
    {
        abilityLock[index].SetActive(false);
    }

    public void TurretPurchasePanel(bool status)
    {
        turretPurchasePanelGameObject.SetActive(status);
    }

    #endregion

    #region Update UI Methods

    public void UpdateUIElements()
    {
        if (enemyCount != null) enemyCount.text = "" + GameManager.Instance._enemyCount;
        if (playerScore != null) playerScore.text = "" + GameManager.Instance._playerScore;
        if (waveLevel != null) waveLevel.text = "" + GameManager.Instance._waveLevel;
    }

    public void UpdateTowerDetails(float towerHealth)
    {
        if (towerHealthBar != null) towerHealthBar.fillAmount = towerHealth;
    }

    public void UpdateWaveBar(float value, bool status)
    {
        waveBar.gameObject.SetActive(status);
        if (waveBar != null && status) waveBar.fillAmount = value;
    }

    public void UpdateInvetory(int id, int qty)
    {
        if (id < 0 || id >= inventoryTexts.Length) return;
        inventoryTexts[id].text = qty.ToString();
    }

    #endregion

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
