using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TMP_Text shopCoin;
    public Image towerHealthBar,
                 dayNightBar;
    public GameObject inventoryPanel,
                      waveModePanel,
                      buildModePanel,
                      skipTheNightButton,
                      skipTheDayButton,
                      infoBox,
                      comboBox,
                      buildThingsPanel;
    [SerializeField]
    private PlayerTower playerTower;
    [SerializeField]
    private InventoryManager inventoryManager;
    public Text[] inventoryTexts;

    #endregion

    #region Unity Methods

    void Start()
    {
        inventoryPanel.SetActive(false);
        infoBox.SetActive(false);
        comboBox.SetActive(false);
        skipTheNightButton.SetActive(false);
        skipTheDayButton.SetActive(true);
        buildThingsPanel.SetActive(false);
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
        //TowerUpgradePanel(!towerUpgradePanel.activeSelf);
        GameManager.Instance.OpenShop();
    }

    public void BuildModeButton()
    {
        GameManager.Instance.BuildMode();
        buildThingsPanel.SetActive(GameManager.Instance.isBuildMode);
        inventoryPanel.SetActive(GameManager.Instance.isBuildMode);
    }

    #endregion

    #region Update UI Methods

    public void BuildModeStatus(bool status)
    {
        
    }

    public void UpdateUIElements()
    {
        if (enemyCount != null) enemyCount.text = "" + GameManager.Instance._enemyCount;
        if (playerScore != null) playerScore.text = "" + GameManager.Instance._playerScore;
        if (shopCoin != null) shopCoin.text = "" + GameManager.Instance._playerScore;
        if (waveLevel != null) waveLevel.text = "" + GameManager.Instance._currentDayCount;
        GameManager.Instance.SaveCurrentPlayerData();
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
        GameManager.Instance.SaveCurrentPlayerData();
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
        skipTheDayButton.SetActive(!status);
        waveModePanel.SetActive(status);
        if (!status) skipTheNightButton.SetActive(status);
        buildModePanel.SetActive(!status);
    }
}
