using UnityEngine;

public class PlayerTower : MonoBehaviour, IDamagable
{
    #region Variables

    private int _towerHealth,
                _maxTowerHealth;
    public float _fireRate { get ; private set; }
    public float _shooterRange { get; private set; }
    public float _bulletDamage { get; private set; }
    [SerializeField]
    private TowerShooter towerShooter;
    [SerializeField]
    private Animator towerAnimator;

    #endregion

    #region Unity Methods

    void Start()
    {
        _towerHealth = 500;
        _maxTowerHealth = 500;
        _fireRate = 5f;
        _shooterRange = 10f;
        _bulletDamage = 1f;
        UpdateTowerUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            towerAnimator.SetTrigger("Hit");
            _towerHealth--;
            UpdateTowerUI();
            if (_towerHealth <= 0)
            {
                towerAnimator.SetTrigger("Dead");
                GameManager.Instance.GameOver();
            }
        }
    }

    void IDamagable.GotHit(float damage)
    {
        towerAnimator.SetTrigger("Hit");
        _towerHealth-= (int)damage;
        Debug.Log("Tower got hit");
        UpdateTowerUI();
        if (_towerHealth <= 0)
        {
            towerAnimator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }

    bool IDamagable.isDamagable()
    {
        return _towerHealth > 0;
    }

    #endregion

    #region Upgrade Methods

    public void TowerHealthUpgrade()
    {
        _maxTowerHealth += 10;
        _towerHealth += _maxTowerHealth;
        UpdateTowerUI();
        DebugForCheck();
    }

    public void TowerFireRateUpgrade()
    {
        if (_fireRate > 0.5f)
        {
            _fireRate -= 0.25f;
        }
        DebugForCheck();
    }

    public void TowerDamageUpgrade()
    {
        _bulletDamage += 1f;
        DebugForCheck();
    }

    public void TowerProductivityUpgrade()
    {
        DebugForCheck();
    }

    public void DebugForCheck()
    {
        Debug.Log("Tower Upgraded");
    }

    #endregion

    public void GameOver()
    {
        // _towerHealth = _maxTowerHealth;
        UpdateTowerUI();
    }

    void UpdateTowerUI()
    {
        UIManager.Instance.UpdateTowerDetails((float)_towerHealth / _maxTowerHealth);
    }

    public int GetTowerHealth()
    {
        return _towerHealth;
    }

    public void SetTowerHealth(int health)
    {
        _towerHealth = health;
        UpdateTowerUI();
    }

    public void SetMaxTowerHealth()
    {
        _towerHealth = _maxTowerHealth;
        UpdateTowerUI();
    }
}
