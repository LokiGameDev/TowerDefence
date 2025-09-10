using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float shootInterval = 1f;
    [SerializeField]
    private Transform projectileSpawnPos;
    private bool canShoot;
    private GameObject turretMesh;

    private List<Transform> targets = new();

    private Transform currentTarget = null;

    void Start()
    {
        turretMesh = transform.GetChild(0).gameObject;
        canShoot = true;
    }

    private void FindAllTargets()
    {
        targets.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            targets.Add(enemy.transform);
        }
    }

    private void FindTheClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (var target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        currentTarget = closestTarget;
    }

    void Update()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeSelf)
        {
            FindAllTargets();
            FindTheClosestTarget();
        }
        if (currentTarget != null && currentTarget.gameObject.activeSelf)
        {
            turretMesh.transform.LookAt(currentTarget);
            if (canShoot)
            {
                var bullet = Instantiate(projectilePrefab, projectileSpawnPos.position, projectilePrefab.transform.rotation);
                bullet.GetComponent<TowerBullet>().AttackTheTarget(currentTarget.gameObject);
                canShoot = false;
                StartCoroutine(ShootCooldown());
            }
        }
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }
}
