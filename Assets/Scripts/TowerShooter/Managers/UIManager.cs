using System.Collections;
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
                infoDisplay,
                comboText,
                waveLevel;
    public Image towerHealthBar,
                 dayNightBar;
    public GameObject towerUpgradePanel,
                      turretPurchasePanelGameObject,
                      inventoryPanel,
                      waveModePanel,
                      menuModePanel,
                      buildModePanel,
                      turretUpgradePanel,
                      skipTheNightButton,
                      skipTheDayButton,
                      infoBox,
                      comboBox,
                      buildThingsPanel;

    public GameObject[] abilityLock;
    private GameObject currentUpgradeObject;
    [SerializeField]
    private PlayerTower playerTower;
    [SerializeField]
    private InventoryManager inventoryManager;
    public Text[] inventoryTexts;

    #endregion

    #region Unity Methods

    void Start()
    {
        towerUpgradePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        infoBox.SetActive(false);
        comboBox.SetActive(false);
        skipTheNightButton.SetActive(false);
        skipTheDayButton.SetActive(true);
        turretUpgradePanel.SetActive(false);
        foreach (var abilityLockObj in abilityLock)
        {
            abilityLockObj.SetActive(true);
        }
        turretPurchasePanelGameObject.SetActive(false);
        buildThingsPanel.SetActive(false);
    }

    void Update()
    {
        if(towerUpgradePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            TowerUpgradePanel(false);
        }
    }
    #endregion

    #region Button Methods

    public void RestartButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void UpgradeButton()
    {
        if (GameManager.Instance.isBuildMode) BuildModeButton();
        TowerUpgradePanel(!towerUpgradePanel.activeSelf);
    }

    public void BuildModeButton()
    {
        if (towerUpgradePanel.activeSelf) UpgradeButton();
        GameManager.Instance.BuildMode();
        buildThingsPanel.SetActive(GameManager.Instance.isBuildMode);
        inventoryPanel.SetActive(GameManager.Instance.isBuildMode);
    }

    #endregion

    #region Tower Upgrade Methods

    public void TowerUpgradePanel(bool status)
    {
        towerUpgradePanel.SetActive(status);
        if (!status) // For return to game
        {
            turretPurchasePanelGameObject.SetActive(status);
            inventoryPanel.SetActive(status);
            infoDisplay.gameObject.SetActive(status);
        }
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

    public void TurretPurchasePanel()
    {
        bool status = !turretPurchasePanelGameObject.activeSelf;
        turretPurchasePanelGameObject.SetActive(status);
        inventoryPanel.SetActive(status);
    }

    #endregion

    public void TurretUpgradePanel(GameObject turret,bool status)
    {
        if(currentUpgradeObject == turret)
        {
            if(status) turretUpgradePanel.SetActive(!turretUpgradePanel.activeSelf);
            else turretUpgradePanel.SetActive(false);
        }
        else
        {
            if(status) turretUpgradePanel.SetActive(true);
            else turretUpgradePanel.SetActive(false);
            currentUpgradeObject = turret;
        }
    }

    #region Update UI Methods

    public void BuildModeStatus(bool status)
    {
        if(status)
        {
            turretUpgradePanel.SetActive(false);
        }
    }

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
        dayNightBar.gameObject.SetActive(status);
        if (dayNightBar != null && status) dayNightBar.fillAmount = value;
    }

    public void UpdateInvetory(int id, int qty)
    {
        if (id < 0 || id >= inventoryTexts.Length) return;
        inventoryTexts[id].text = qty.ToString();
    }

    public void DisplayInformation(string msg)
    {
        if (!infoBox.activeSelf)
        {
            infoBox.SetActive(true);
            infoDisplay.text = msg;
            StartCoroutine(DisplayMessageCooldown());
        }
        else
        {
            infoDisplay.text = msg;
        }
    }

    #endregion

    private Coroutine comboCoroutine;
    public void AddToCombo(int value)
    {
        if(!comboBox.activeSelf) comboBox.SetActive(true);
        comboText.text = "+" + value + " Combo";
        if(comboCoroutine!=null)
        {
            comboBox.GetComponent<CanvasGroup>().alpha = 1;
            StopCoroutine(comboCoroutine);
        }
        comboCoroutine = StartCoroutine(FadeOutCombo());
    }

    IEnumerator FadeOutCombo()
    {
        yield return new WaitForSeconds(GameManager.Instance._comboInterval);

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            comboBox.GetComponent<CanvasGroup>().alpha = 1f - (t / duration);
            yield return null;
        }

        comboBox.GetComponent<CanvasGroup>().alpha = 0f;
        comboBox.SetActive(false);
    }

    public void AllEnemiesAreCleared()
    {
        skipTheNightButton.SetActive(true);
    }

    IEnumerator DisplayMessageCooldown()
    {
        yield return new WaitForSeconds(2);
        infoBox.gameObject.SetActive(false);
    }

    public void WaveStatus(bool status)
    {
        if(status)
        {
            turretUpgradePanel.SetActive(false);
        }
        skipTheDayButton.SetActive(!status);
        waveModePanel.SetActive(status);
        if (!status) skipTheNightButton.SetActive(status);
        buildModePanel.SetActive(!status);
        menuModePanel.SetActive(!status);
        if(status && towerUpgradePanel.activeSelf) TowerUpgradePanel(!towerUpgradePanel.activeSelf);
    }
}
