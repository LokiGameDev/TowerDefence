using System.Collections;
using UnityEngine;

public class AreaDamageTurret : PlayerTurret, ISpecialEffect
{
    public TurretAreaDamager areaDamager;
    private GameObject turretMesh;

    void Start()
    {
        turretMesh = transform.GetChild(0).gameObject;
        InitialiseTurretData(3);
        canShoot = true;
        areaDamager?.SetTheRange(_attackRadius*2);
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
                if (canShoot)
                {
                    areaDamager?.AttackAllEnemies(_turretDamage);
                    canShoot = false;
                    StartCoroutine(ShootCooldown());
                }
            }
        }
    }

    public void UpgradeTurretData()
    {
        InitialiseTurretData(3);
        areaDamager?.SetTheRange(_attackRadius*2);
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
