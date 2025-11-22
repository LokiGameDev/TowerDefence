using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    #region Variables

    private int _towerHealth;
    private int _maxTowerHealth;
    private bool _canUpgradeHealth;
    private bool _canBuyTurrets;
    [SerializeField]
    private TowerShooter towerShooter;

    #endregion

    #region Unity Methods

    void Start()
    {
        _maxTowerHealth = 20;
        _towerHealth = 20;
        _canUpgradeHealth = false;
        _canBuyTurrets = false;
        UpdateTowerUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _towerHealth--;
            UpdateTowerUI();
            if (_towerHealth <= 0)
            {
                GameManager.Instance.WaveOver();
            }
        }
    }

    #endregion

    #region Upgrade Methods

    public void TowerHealthUpgrade()
    {
        if (GameManager.Instance.Purchasing(20) && _canUpgradeHealth)
        {
            _maxTowerHealth++;
            UpdateTowerUI();
        }
        else if (GameManager.Instance.Purchasing(30))
        {
            _canUpgradeHealth = true;
            UIManager.Instance.AbilityUnlock(1);
        }
    }

    public void TowerAttackSpeedUpgrade()
    {
        if (GameManager.Instance._playerScore >= 15 && towerShooter._isAbilityUnlocked)
        {
            bool upgraded = GameObject.Find("TowerShooter").GetComponent<TowerShooter>().ReduceCollDownUpgrade();
            if (upgraded) GameManager.Instance.Purchasing(15);
            else Debug.Log("Already at max level");
        }
        else if (!towerShooter._isAbilityUnlocked && GameManager.Instance.Purchasing(10))
        {
            towerShooter.UnlockAttackAbility();
            UIManager.Instance.AbilityUnlock(0);
        }
    }

    public void TowerTurretUpgrade()
    {
        if (_canBuyTurrets)
        {
            UIManager.Instance.TurretPurchasePanel();
        }
        else if (!_canBuyTurrets && GameManager.Instance.Purchasing(25))
        {
            _canBuyTurrets = true;
            UIManager.Instance.AbilityUnlock(2);
        }
    }

    #endregion

    public void GameOver()
    {
        _towerHealth = _maxTowerHealth;
        UpdateTowerUI();
    }

    void UpdateTowerUI()
    {
        UIManager.Instance.UpdateTowerDetails((float)_towerHealth / _maxTowerHealth);
    }
}
