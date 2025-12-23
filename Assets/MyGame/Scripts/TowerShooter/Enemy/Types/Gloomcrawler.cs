using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloomcrawler : Enemy
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
    private bool _isMoving, _isSpawning;
    public GameObject projectilePrefab;
    public GameObject smallBugPrefab;

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base.localMaxHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
        base.isStunned = false;
        canShoot = true;
        _currentTarget = null;
        _isMoving = false;
        _isSpawning = false;
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
            if(!_isMoving)
            {
                _isMoving = true;
                _isSpawning = true;
                Debug.Log("Spawning");
                StartCoroutine(SpawnSmallBugDelay());
            }
            else if(!_isSpawning)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
                transform.LookAt(targetPosition);
            }
        }
    }

    private void SpawnTheBugs(int count)
    {
        for(int i=0;i<count;i++)
        {
            var enemy = Instantiate(smallBugPrefab, transform.position, smallBugPrefab.transform.rotation);
            enemy.gameObject.transform.position = GenerateRandomSpawnLoc();
            enemy.gameObject.SetActive(true);
        }
    }

    private Vector3 GenerateRandomSpawnLoc()
    {
        float x1,x2,z1,z2;
        x1 = transform.position.x - 2;
        x2 = transform.position.x + 2;
        z1 = transform.position.z - 2;
        z2 = transform.position.z + 2;

        Vector3 pos = new Vector3(Random.Range(x1,x2), transform.position.y - 0.2f, Random.Range(z1,z2));

        return pos;
    }

    IEnumerator SpawnSmallBugDelay()
    {
        SpawnTheBugs(1);
        yield return new WaitForSeconds(2);
        _isSpawning=false;
        yield return new WaitForSeconds(5);
        _isMoving=false;
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(3);
        canShoot = true;
    }
}