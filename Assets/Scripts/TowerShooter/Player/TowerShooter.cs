using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooter : MonoBehaviour
{

    #region Variables

    public bool _isAbilityUnlocked { get; private set; }
    private bool _canShoot;
    private float _bulletSpeed = 5f;
    private float _shootCoolDown, _minShootCoolDown = 1;
    public GameObject bulletPrefab;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    #endregion

    #region Unity Methods

    void Start()
    {
        _shootCoolDown = 5;
        _canShoot = true;
        _isAbilityUnlocked = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void Update()
    {
        var target = GetClosestEnemy();
        if (target != null && target.activeSelf && _canShoot && _isAbilityUnlocked)
        {
            transform.LookAt(target.transform);
            var bullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
            bullet.GetComponent<Bullet>().BulletSpeedSetUp(_bulletSpeed);
            bullet.GetComponent<Bullet>().AttackTheTarget(target, "Enemy");
            _canShoot = false;
            StartCoroutine(ShootCoolDown());
        }
    }

    #endregion

    #region Custom Methods

    GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;

        float minDistance = 35f;

        foreach (var enemy in enemiesInRange)
        {
            if(enemy==null) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (minDistance > distance)
            {
                closestEnemy = enemy;
                minDistance = distance;
            }
        }

        return closestEnemy;
    }

    #endregion

    #region Upgrade Methods

    public bool ReduceCollDownUpgrade()
    {
        if (_shootCoolDown <= _minShootCoolDown) return false;
        else
        {
            _shootCoolDown -= 1;
            return true;
        }
    }

    public void UnlockAttackAbility()
    {
        _isAbilityUnlocked = true;
    }

    #endregion

    #region Coroutines

    IEnumerator ShootCoolDown()
    {
        yield return new WaitForSeconds(_shootCoolDown);
        _canShoot = true;
    }
    
    #endregion
}
