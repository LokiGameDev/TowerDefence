using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    #region Variables

    private int _towerHealth;
    private int _maxTowerHealth;

    #endregion

    #region Unity Methods

    void Start()
    {
        _maxTowerHealth = 20;
        _towerHealth = 10;
        UpdateTowerUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _towerHealth--;
            UpdateTowerUI();
            other.GetComponent<Enemy>().GotKilled();
            if (_towerHealth <= 0)
            {
                GameOver();
            }
        }
    }

    #endregion

    #region Upgrade Methods
    
    public void TowerHealthUpgrade()
    {
        if (GameManager.Instance._playerScore >= 20)
        {
            _maxTowerHealth++;
            UpdateTowerUI();
            GameManager.Instance.Purchasing(20);
        }
    }

    public void TowerAttackSpeedUpgrade()
    {
        if(GameManager.Instance._playerScore >= 15 && GameManager.Instance._attackAbility)
        {
            bool upgraded = GameObject.Find("TowerShooter").GetComponent<TowerShooter>().ReduceCollDownUpgrade();
            if (upgraded) GameManager.Instance.Purchasing(15);
            else Debug.Log("Already at max level");
        }
        else if (!GameManager.Instance._attackAbility && GameManager.Instance._playerScore >= 10)
        {
            GameManager.Instance.Purchasing(10);
            GameManager.Instance.UnlockAttackAbility();
        }
    }

    public void TowerTurretUpgrade()
    {
        if(GameManager.Instance._playerScore >= 30)
        {
            
        }
    }

    #endregion

    void GameOver()
    {
        GameManager.Instance.GameOver();
    }

    void UpdateTowerUI()
    {
        UIManager.Instance.UpdateTowerDetails((float)_towerHealth / _maxTowerHealth);
    }
}
