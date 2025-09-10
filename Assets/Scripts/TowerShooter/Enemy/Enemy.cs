using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    [SerializeField]
    public int _enemyValue { get;  private set; }
    private float _speed;
    private bool _isTowerAvailable;
    private GameObject _playerTower;

    #endregion

    #region Unity Methods

    void Start()
    {
        _enemyValue = 1;
        _speed = 2f;
    }

    void OnEnable()
    {
        _playerTower = GameObject.Find("PlayerTower");
        _isTowerAvailable = _playerTower != null ? true : false;
    }

    void Update()
    {
        if (_isTowerAvailable) MoveTowardsTower();
    }

    #endregion

    #region Enemy Methods

    void MoveTowardsTower()
    {
        var targetPosition = new Vector3(_playerTower.transform.position.x, transform.position.y, _playerTower.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); ;
    }

    public void GotKilled(bool isScore)
    {
        EnemyPool.Instance.ReturnToPool(this);
        if(isScore) GameManager.Instance.EnemyGotDestroyed();
    }
    
    #endregion
}
