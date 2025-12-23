using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photonfly : Enemy
{
    private GameObject _currentTarget;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int enemyValue;
    [SerializeField]
    private float enemyHealth;
    [SerializeField]
    private float attackRange;
    private bool canShoot;
    public GameObject projectilePrefab;
    private float shootInterval;

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base.localMaxHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
        base.isStunned = false;
        canShoot = true;
        _currentTarget = null;
        shootInterval = 5f;
        SetEnemyHealthBar();
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentTarget==null || !_currentTarget.activeSelf) _currentTarget = FindClosestTarget();
        if (_currentTarget!=null) GoNearTheTarget();
        else Debug.Log("Shit");
    }

    
    void GoNearTheTarget()
    {
        if(!isAlive  || isStunned) return;
        var targetPosition = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
        if(Vector3.Distance(transform.position, targetPosition) < 5)
        {
            enemyAnimation.EnemyAnimationTrigger("Attack");
            if (canShoot)
            {
                canShoot = false;
                StartCoroutine(ShootTheFreezer());
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(targetPosition); 
        }
    }

    IEnumerator ShootTheFreezer()
    {
        yield return new WaitForSeconds(shootInterval);
        var spawnPos = new Vector3 (transform.position.x , _currentTarget.transform.position.y, transform.position.z);
        var bullet = Instantiate(projectilePrefab, spawnPos, projectilePrefab.transform.rotation);
        bullet.GetComponent<Bullet>().BulletSpeedSetUp(3);
        bullet.GetComponent<Bullet>().AttackTheTarget(_currentTarget, _currentTarget.gameObject.tag);
        bullet.GetComponent<Bullet>().SpecialEffect();
        canShoot = true;
    }
}
