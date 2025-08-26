using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooter : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float _shootCoolDown;
    public GameObject bulletPrefab;
    private bool _canShoot;

    void Start()
    {
        _shootCoolDown = 5;
        _canShoot = true;
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
        if (target != null && target.activeSelf && _canShoot && GameManager.Instance._attackAbility)
        {
            transform.LookAt(target.transform);
            var bullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
            bullet.GetComponent<TowerBullet>().AttackTheTarget(target);
            _canShoot = false;
            StartCoroutine(ShootCoolDown());
        }
    }

    GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;

        float minDistance = 35f;

        foreach (var enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (minDistance > distance)
            {
                closestEnemy = enemy;
                minDistance = distance;
            }
        }

        return closestEnemy;
    }

    IEnumerator ShootCoolDown()
    {
        yield return new WaitForSeconds(_shootCoolDown);
        _canShoot = true;
    }

    public void ReduceCollDownUpgrade()
    {
        _shootCoolDown -= 2;
    }
}
