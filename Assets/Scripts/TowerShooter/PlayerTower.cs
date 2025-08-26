using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    private int _towerHealth;
    private int _maxTowerHealth;

    void Start()
    {
        _maxTowerHealth = 10;
        _towerHealth = 10;
        UpdateTowerUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _towerHealth--;
            UpdateTowerUI();
            other.GetComponent<Enemy>().DamagedTheTower();
            if (_towerHealth <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        GameManager.Instance.GameOver();
    }

    void UpdateTowerUI()
    {
        UIManager.Instance.UpdateTowerDetails((float)_towerHealth / _maxTowerHealth);
    }

    public void PlayerTowerHealthUpgrade(int value)
    {
        _maxTowerHealth += value;
        UIManager.Instance.UpdateTowerDetails((float)_towerHealth / _maxTowerHealth);
    }
}
