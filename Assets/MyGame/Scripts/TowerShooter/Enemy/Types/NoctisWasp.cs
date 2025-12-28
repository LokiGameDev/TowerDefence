using System.Collections;
using UnityEngine;

public class NoctisWasp : Enemy
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
    [SerializeField]
    private float enemyDamage;
    private bool canShoot;
    public GameObject projectilePrefab;

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base.localMaxHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
        base.isStunned = false;
        base._enemyDamage = enemyDamage;
        canShoot = true;
        _currentTarget = null;
        SetEnemyHealthBar();
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentTarget==null || !_currentTarget.activeSelf || !_currentTarget.GetComponent<IDamagable>().isDamagable()) _currentTarget = FindClosestTarget();
        if (_currentTarget!=null) GoNearTheTarget();
        else Debug.Log("Shit");
    }

    
    void GoNearTheTarget()
    {
        if(!isAlive || isStunned) return;
        var targetPosition = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
        if(Vector3.Distance(transform.position, targetPosition) < attackRange)
        {
            enemyAnimation.EnemyAnimationTrigger("Attack");
            if (canShoot)
            {
                var spawnPos = new Vector3 (transform.position.x , _currentTarget.transform.position.y, transform.position.z);
                var bullet = Instantiate(projectilePrefab, spawnPos, projectilePrefab.transform.rotation);
                bullet.GetComponent<Bullet>().BulletSpeedSetUp(10);
                bullet.GetComponent<Bullet>().AttackTheTarget(_currentTarget, _currentTarget.gameObject.tag);
                bullet.GetComponent<Bullet>().BulletDamageSetup(_enemyDamage);
                canShoot = false;
                StartCoroutine(ShootCooldown());
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(targetPosition); 
        }
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(3);
        canShoot = true;
    }
}
