using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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


    public Text enemyCount;
    public Text playerScore;
    public Text waveLevel;
    public GameObject gameOverPanel;
    public Image towerHealthBar;
    public Image waveBar;
    public GameObject towerUpgradePanel;
    public bool upgradePanelOpen;
    public GameObject abilityLock;

    void Start()
    {
        gameOverPanel.SetActive(false);
        towerUpgradePanel.SetActive(false);
        upgradePanelOpen = false;
        abilityLock.SetActive(true);
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

    public void UpdateWaveBar(float value)
    {
        if (waveBar != null) waveBar.fillAmount = value;
    }

    public void UpdateWaveBar(bool status)
    {
        if (waveBar != null) waveBar.gameObject.SetActive(status);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void UpgradeButton()
    {
        TowerUpgradePanel(!towerUpgradePanel.activeSelf);
    }

    public void TowerUpgradePanel(bool status)
    {
        towerUpgradePanel.SetActive(status);
        upgradePanelOpen = status;
        GameManager.Instance.GamePauseStatus(!status);
    }

    public void TowerHealthUpgrade()
    {
        if (GameManager.Instance._playerScore > 5)
        {
            GameManager.Instance.Purchasing(5);
            GameObject.Find("PlayerTower").GetComponent<PlayerTower>().PlayerTowerHealthUpgrade(1);
        }
    }

    public void TowerAttackSpeedUpgrade()
    {
        if (GameManager.Instance._playerScore >= 5 && GameManager.Instance._attackAbility)
        {
            GameManager.Instance.Purchasing(5);
            GameObject.Find("TowerShooter").GetComponent<TowerShooter>().ReduceCollDownUpgrade();
        }
        else if (!GameManager.Instance._attackAbility && GameManager.Instance._playerScore >= 10)
        {
            GameManager.Instance.Purchasing(10);
            GameManager.Instance.UnlockAttackAbility();
            abilityLock.SetActive(false);
        }
    }

    public void TowerTurretUpgrade()
    {

    }
}
