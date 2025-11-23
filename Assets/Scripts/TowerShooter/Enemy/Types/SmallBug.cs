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

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
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
        if(!isAlive) return;
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
        }
    }

    public override void GotHit()
    {
        Destroy(this.gameObject);
    }
}
