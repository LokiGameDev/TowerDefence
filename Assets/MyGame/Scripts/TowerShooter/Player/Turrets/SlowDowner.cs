using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDowner : PlayerTurret, ISpecialEffect
{
    [SerializeField]
    protected TurretSlowDowner turretSlowDowner;
    [SerializeField]
    protected GameObject attackRange;
    [SerializeField]
    protected GameObject slowDownArea;
    private GameObject turretMesh;

    void Start()
    {
        turretMesh = transform.GetChild(0).gameObject;
        InitialiseTurretData(1);
        canShoot = true;
        slowDownArea.transform.localScale = new Vector3(slowDownArea.transform.localScale.x, slowDownArea.transform.localScale.y, _attackRadius);
        attackRange.transform.localScale = new Vector3(_attackRadius*2, attackRange.transform.localScale.y, _attackRadius*2);
        ShopManager.turretUpgradeEvent += UpgradeTurretData;
    }

    void Update()
    {
        base.FindAllTargets();
        base.FindTheClosestTarget();
        if (currentTarget != null && currentTarget.gameObject.activeSelf)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) < _attackRadius)
            {
                var currentTargetLook = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
                turretMesh.transform.LookAt(currentTargetLook);
                slowDownArea.transform.LookAt(currentTargetLook);
                if (canShoot)
                {
                    Debug.Log("Slow Downer");
                    turretSlowDowner?.AttackAllEnemies(_turretDamage);
                    canShoot = false;
                    StartCoroutine(ShootCooldown());
                }
            }
        }
    }

    public void UpgradeTurretData()
    {
        InitialiseTurretData(1);
        slowDownArea.transform.localScale = new Vector3(slowDownArea.transform.localScale.x, slowDownArea.transform.localScale.y, _attackRadius);
        attackRange.transform.localScale = new Vector3(_attackRadius*2, attackRange.transform.localScale.y, _attackRadius*2);
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(_shootInterval);
        canShoot = true;
    }

    void ISpecialEffect.Stunned()
    {
        StopAllCoroutines();
        canShoot=false;
        specialEffectObject.SetActive(true);
        if(this.gameObject.activeSelf) StartCoroutine(GotStunned());
    }

    IEnumerator GotStunned()
    {
        yield return new WaitForSeconds(5);
        canShoot=true;
        specialEffectObject.SetActive(false);
    }
}
