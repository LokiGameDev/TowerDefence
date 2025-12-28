using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurret : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected float _shootInterval = 1f;
    protected string _turretName = "null";
    protected float _bulletSpeed = 10f;
    [SerializeField]
    protected float _turretHealth = 1f;
    protected float _attackRadius;
    protected int _turretDamage = 10;

    protected bool canShoot;
    protected bool _canUpgrade;

    protected Transform currentTarget = null;
    protected List<Transform> targets = new();

    [SerializeField]
    protected Transform attackRangeCenter;
    [SerializeField]
    protected GameObject specialEffectObject;

    [SerializeField]
    protected Image healthImage;
    protected float localMaxHealth;

    [SerializeField]
    protected GameObject turretExplosion;

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
        if (other.CompareTag("Enemy") && _turretHealth > 0)
        {
            _turretHealth--;
            healthImage.fillAmount = _turretHealth/localMaxHealth;
            if (_turretHealth <= 0)
            {
                canShoot = false;
                Instantiate(turretExplosion,transform.position + new Vector3(1,0,1), Quaternion.identity);
            }
        }
    }

    void IDamagable.GotHit(float damage)
    {
        if(_turretHealth <= 0)
        {
            _turretHealth = 0;
            canShoot = false;
            return;
        }
        _turretHealth-=(int)damage;
        if(_turretHealth <=0) AudioManager.Instance.PlayTheAudioClip(AudioType.TurretDestroyed);
        healthImage.fillAmount = _turretHealth/localMaxHealth;
        Debug.Log("Hit");
        if (_turretHealth <= 0)
        {
            canShoot = false;
            Instantiate(turretExplosion,transform.position + new Vector3(1,0,1), Quaternion.identity);
        }
    }

    bool IDamagable.isDamagable()
    {
        return _turretHealth > 0;
    }

    public void TurretUpgradeStatus(bool status)
    {
        _canUpgrade = status;
    }

    void OnMouseDown()
    {
        UIManager.Instance.ShowTheTurretDetails(true, this.gameObject.GetComponent<PlayerTurret>()); 
        AudioManager.Instance.PlayTheAudioClip(AudioType.MouseClick);
    }

    public void RepairTheTurret()
    {
        _turretHealth = localMaxHealth;
        healthImage.fillAmount = _turretHealth/localMaxHealth;
        canShoot = true;
    }

    public TurretData GetTurretDetails()
    {
        TurretData turretData = new TurretData(_turretName, _turretHealth, _turretDamage, _shootInterval, _attackRadius);
        return turretData;
    }

    public void InitialiseTurretData(int turretID)
    {
        TurretData turretData = GameManager.Instance.GetTurretData(turretID);
        if(turretData == null)
        {
            Debug.LogError("Turret Data not found for Turret ID: " + turretID);
            return;
        }
        _turretName = turretData.name;
        _turretHealth = turretData.health;
        localMaxHealth = _turretHealth;
        _turretDamage = turretData.damage;
        _shootInterval = turretData.fireRate * 5;
        _attackRadius = turretData.range;
        healthImage.fillAmount = _turretHealth/localMaxHealth;
    }

    #endregion
}
