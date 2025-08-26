using System.Collections;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private GameObject target;
    private bool _canAttack = false;

    public void AttackTheTarget(GameObject enemy)
    {
        target = enemy;
        transform.LookAt(target.transform);
        _canAttack = true;
    }

    void Update()
    {
        if (_canAttack)
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        }
        if (transform.position.x > 30 || transform.position.z > 30 || transform.position.x < -30 || transform.position.z < -30)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GotKilled();
            Destroy(this.gameObject);
        }
    }
}
