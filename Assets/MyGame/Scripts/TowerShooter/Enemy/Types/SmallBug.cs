using UnityEngine;

public class SmallBug : Enemy
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
        base._enemyDamage = enemyDamage;
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentTarget==null) _currentTarget = FindClosestTarget();
        if (_currentTarget!=null) GoNearTheTarget();
        else Debug.Log("Shit");
    }

    
    void GoNearTheTarget()
    {
        if(!isAlive || isStunned) return;
        //enemyAnimation.EnemyAnimationTrigger("Attack");
        var targetPosition = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTurret") || other.CompareTag("PlayerTower"))
        {
            Destroy(this.gameObject);
            other.GetComponent<IDamagable>()?.GotHit(_enemyDamage);
        }
    }

    public override void GotHit(float damage)
    {
        isAlive=false;
        enemyAnimation.EnemyAnimationTrigger("Dead");
        InitiateTheDeathEffect();
        Destroy(this.gameObject,1.5f);
    }
}
