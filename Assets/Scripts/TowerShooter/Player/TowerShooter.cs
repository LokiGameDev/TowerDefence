using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooter : MonoBehaviour
{

    #region Variables

    private bool _canShoot;
    private float _bulletSpeed = 5f;
    public PlayerTower playerTower;
    public GameObject bulletPrefab;

    #endregion

    #region Unity Methods

    void Start()
    {
        _canShoot = true;
    }

    void Update()
    {
        var target = FindClosestTargetInRange();
        if (target != null && target.activeSelf && _canShoot)
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

    private GameObject FindClosestTargetInRange()
    {
        GameObject closestEnemy = null;

        float minDistance = Mathf.Infinity;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= playerTower._shooterRange)
            {
                enemiesInRange.Add(enemy);
            }
        }

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

    #region Coroutines

    IEnumerator ShootCoolDown()
    {
        yield return new WaitForSeconds(playerTower._fireRate);
        _canShoot = true;
    }
    
    #endregion
}
