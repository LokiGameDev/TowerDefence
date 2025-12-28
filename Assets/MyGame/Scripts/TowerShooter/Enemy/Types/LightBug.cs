using UnityEngine;

public class LightBug : Enemy
{
    private GameObject _currentTarget;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int enemyValue;
    [SerializeField]
    private float enemyHealth;
    [SerializeField]
    private float enemyDamage;

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base.localMaxHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
        base.isStunned = false;
        base._enemyDamage = enemyDamage;
        SetEnemyHealthBar();
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentTarget==null || !_currentTarget.GetComponent<IDamagable>().isDamagable()) _currentTarget = FindClosestTarget();
        if (_currentTarget!=null) GoNearTheTarget();
        else Debug.Log("Shit");
    }

    
    void GoNearTheTarget()
    {
        if(!isAlive || isStunned || canMoveForTest) return;
        //enemyAnimation.EnemyAnimationTrigger("Normal");
        var targetPosition = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTurret") || other.CompareTag("PlayerTower"))
        {
            if(other.GetComponent<IDamagable>()!=null)
            {
                if(!other.GetComponent<IDamagable>().isDamagable()) return;
            }
            Sacrificed();
            other.GetComponent<IDamagable>()?.GotHit(_enemyDamage);
        }
    }
}
