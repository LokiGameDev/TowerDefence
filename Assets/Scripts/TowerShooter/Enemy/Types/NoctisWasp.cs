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
    private bool canShoot;
    public GameObject projectilePrefab;

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
        canShoot = true;
        _currentTarget = null;
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
        if(!isAlive) return;
        var targetPosition = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
        if(Vector3.Distance(transform.position, targetPosition) < 5)
        {
            enemyAnimation.EnemyAnimationTrigger("Attack");
            if (canShoot)
            {
                var spawnPos = new Vector3 (transform.position.x , _currentTarget.transform.position.y, transform.position.z);
                var bullet = Instantiate(projectilePrefab, spawnPos, projectilePrefab.transform.rotation);
                bullet.GetComponent<Bullet>().BulletSpeedSetUp(3);
                bullet.GetComponent<Bullet>().AttackTheTarget(_currentTarget, "Player");
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
