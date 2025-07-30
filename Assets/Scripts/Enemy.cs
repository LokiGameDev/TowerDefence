using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _playerTower;
    private float _speed;

    void Start()
    {
        _speed = 2f;
    }
    void OnEnable()
    {
        _playerTower = GameObject.Find("PlayerTower");
    }

    void Update()
    {
        var targetPosition = new Vector3(_playerTower.transform.position.x, transform.position.y, _playerTower.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    }

    public void DamagedTheTower()
    {
        EnemyPool.Instance.ReturnToPool(this);
    }

    public void GotKilled()
    {
        EnemyPool.Instance.ReturnToPool(this);
    }
}
