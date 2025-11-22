using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    #region Variables

    private float _bulletSpeed = 5f,
                    _maxMovingDistance = 30f;
    private Vector3 _spawnPos;

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        _spawnPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
        if (Vector3.Distance(_spawnPos, transform.position) > _maxMovingDistance) Destroy(this.gameObject);
    }

    #endregion

    #region Methods

    public void AttackTheTarget(GameObject enemy)
    {
        transform.LookAt(enemy.transform);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GotHit();
            Destroy(this.gameObject);
        }
    }

    public void BulletSpeedSetUp(float speed)
    {
        _bulletSpeed = speed;
    }

    #endregion
}
