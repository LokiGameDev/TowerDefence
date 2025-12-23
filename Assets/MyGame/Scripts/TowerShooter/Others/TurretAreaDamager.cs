using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAreaDamager : MonoBehaviour
{
    List<GameObject> enemiesInside = new List<GameObject>();
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(!enemiesInside.Contains(other.gameObject))
            {
                enemiesInside.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(enemiesInside.Contains(other.gameObject))
            {
                enemiesInside.Remove(other.gameObject);
            }
        }
    }

    public void AttackAllEnemies(int damage)
    {
        foreach (var enemy in enemiesInside)
        {
            if(enemy.activeSelf) enemy.GetComponent<IDamagable>()?.GotHit(damage);
        }
        enemiesInside.Clear();
    }

    public void SetTheRange(float range)
    {
        transform.localScale = new Vector3(range, transform.localScale.y, range);
    }
}
