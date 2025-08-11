using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    private int _towerHealth;

    void Start()
    {
        _towerHealth = 10;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _towerHealth--;
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
}
