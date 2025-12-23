
using System.Collections;
using UnityEngine;

public class NormalTurret : PlayerTurret, ISpecialEffect
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected Transform projectileSpawnPos;
    [SerializeField]
    protected GameObject attackRange;
    private GameObject turretMesh;

    void Start()
    {
        turretMesh = transform.GetChild(0).gameObject;
        InitialiseTurretData(0);
        canShoot = true;
        ShopManager.turretUpgradeEvent += UpgradeTurretData;
        attackRange.transform.localScale = new Vector3(_attackRadius*2, attackRange.transform.localScale.y, _attackRadius*2);
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
                    var bullet = Instantiate(projectilePrefab, projectileSpawnPos.position, projectilePrefab.transform.rotation).GetComponent<Bullet>();
                    bullet.BulletSpeedSetUp(_bulletSpeed);
                    bullet.BulletDamageSetup(_turretDamage);
                    bullet.AttackTheTarget(currentTarget.gameObject, "Enemy");
                    canShoot = false;
                    StartCoroutine(ShootCooldown());
                }
            }
        }
    }

    public void UpgradeTurretData()
    {
        InitialiseTurretData(0);
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
