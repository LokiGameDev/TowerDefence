using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    private float _damage = 1;
    private float _bulletSpeed = 5f,
                  _maxMovingDistance = 30f;
    private Vector3 _spawnPos;
    private string targetName;
    private bool isSpecialEffect;

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        _spawnPos = transform.position;
        isSpecialEffect = false;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
        if (Vector3.Distance(_spawnPos, transform.position) > _maxMovingDistance) Destroy(this.gameObject);
    }

    #endregion

    #region Methods

    public void AttackTheTarget(GameObject target, string name)
    {
        transform.LookAt(target.transform);
        targetName = name;

    }


    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamagable>()?.GotHit(_damage);
        if(isSpecialEffect)
        {
            other.GetComponent<ISpecialEffect>()?.Stunned();
        }
        Destroy(this.gameObject);
    }

    public void BulletSpeedSetUp(float speed)
    {
        _bulletSpeed = speed;
    }

    public void BulletDamageSetup(float damage)
    {
        _damage = damage;
    }

    public void SpecialEffect()
    {
        isSpecialEffect = true;
    }

    #endregion
}
