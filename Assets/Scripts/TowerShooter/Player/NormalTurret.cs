
using System.Collections;
using UnityEngine;

public class NormalTurret : PlayerTurret, ISpecialEffect
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected Transform projectileSpawnPos;
    private GameObject turretMesh;

    void Start()
    {
        turretMesh = transform.GetChild(0).gameObject;
        _attackRadius = 10;
        attackRangeRenderer.material.SetFloat("Radius", _attackRadius);
        canShoot = true;
    }

    void Update()
    {
        attackRangeRenderer.material.SetVector("_Center", attackRangeCenter.position);
        base.FindAllTargets();
        base.FindTheClosestTarget();
        if (currentTarget != null && currentTarget.gameObject.activeSelf)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) < _attackRadius)
            {
                turretMesh.transform.LookAt(currentTarget);
                if (canShoot)
                {
                    var bullet = Instantiate(projectilePrefab, projectileSpawnPos.position, projectilePrefab.transform.rotation);
                    bullet.GetComponent<Bullet>().BulletSpeedSetUp(_bulletSpeed);
                    bullet.GetComponent<Bullet>().AttackTheTarget(currentTarget.gameObject, "Enemy");
                    canShoot = false;
                    StartCoroutine(ShootCooldown());
                }
            }
        }
    }
    
    void OnMouseDown()
    {
        if(GameManager.Instance.isBuildMode || GameManager.Instance.IsWaveGoing()) return;
        if(!_canUpgrade)
        {
            UIManager.Instance.TurretUpgradePanel(this.gameObject, false);
            return;
        }
        UIManager.Instance.TurretUpgradePanel(this.gameObject, true);
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
