using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

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
        if(targetName.StartsWith("Player"))
        {
            if (other.CompareTag("PlayerTurret") || other.CompareTag("PlayerTower"))
            {
                other.GetComponent<IDamagable>()?.GotHit();
                if(isSpecialEffect && other.CompareTag("PlayerTurret"))
                {
                    other.GetComponent<ISpecialEffect>()?.Stunned();
                }
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag(targetName))
        {
            other.GetComponent<IDamagable>()?.GotHit();
            Destroy(this.gameObject);
        }
    }

    public void BulletSpeedSetUp(float speed)
    {
        _bulletSpeed = speed;
    }

    public void SpecialEffect()
    {
        isSpecialEffect = true;
    }

    #endregion
}
