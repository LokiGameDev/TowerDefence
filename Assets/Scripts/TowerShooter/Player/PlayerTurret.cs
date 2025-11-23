using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected float _shootInterval = 1f;
    protected float _bulletSpeed = 10f;
    [SerializeField]
    protected float _turretHealth = 1f;
    protected float _attackRadius;

    protected bool canShoot;
    protected bool _canUpgrade;

    protected Transform currentTarget = null;
    protected List<Transform> targets = new();

    [SerializeField]
    protected Transform attackRangeCenter;
    [SerializeField]
    protected Renderer attackRangeRenderer;
    [SerializeField]
    protected GameObject specialEffectObject;

    #region Target methods

    protected void FindAllTargets()
    {
        targets.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            targets.Add(enemy.transform);
        }
    }

    protected void FindTheClosestTarget()
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

    #endregion

    #region Common methods

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _turretHealth--;
            if (_turretHealth <= 0)
            {
                canShoot = false;
                gameObject.SetActive(false);
            }
        }
    }

    void IDamagable.GotHit()
    {
        _turretHealth--;
        if (_turretHealth <= 0)
        {
            canShoot = false;
            gameObject.SetActive(false);
        }
    }

    public void TurretUpgradeStatus(bool status)
    {
        _canUpgrade = status;
    }

    void OnMouseOver()
    {
        
    }

    void OnMouseExit()
    {
        
    }

    public void RepairTurret()
    {
        _turretHealth = 1f;
    }

    public void UpgradeTurret()
    {
        _shootInterval *= 0.9f;
    }

    public void UpgradeTurret(float newBulletSpeed, float newAttackRadius, float newShootInterval)
    {
        _bulletSpeed = newBulletSpeed;
        _attackRadius = newAttackRadius;
        _shootInterval = newShootInterval;
        attackRangeRenderer.material.SetFloat("Radius", _attackRadius);
    }

    #endregion
}
