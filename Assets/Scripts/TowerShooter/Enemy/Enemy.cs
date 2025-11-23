using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    #region Variables

    [SerializeField]
    protected EnemyAnimation enemyAnimation;
    protected bool isAlive = false;
    protected float _enemyHealth;
    protected int _enemyValue;

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        enemyAnimation.EnemyAnimationTrigger("Normal");
    }

    #endregion

    #region Enemy Methods

    public virtual void GotHit()
    {
        if(!isAlive) return;
        _enemyHealth--;
        if(_enemyHealth > 0)
        {
            enemyAnimation.EnemyAnimationTrigger("Hit");
            return;
        }
        else
        {
            isAlive = false;
            enemyAnimation.EnemyAnimationTrigger("Dead");
            GameManager.Instance.EnemyGotDestroyed();
            GameManager.Instance.AddScore(_enemyValue);
            StartCoroutine(ReturnToPoolDelay());
        }
    }

    public void Sacrificed()
    {
        isAlive = false;
        GameManager.Instance.EnemyGotDestroyed();
        EnemyPool.Instance.ReturnToPool(this);
    }

    IEnumerator ReturnToPoolDelay()
    {
        yield return new WaitForSeconds(1.5f);
        EnemyPool.Instance.ReturnToPool(this);
    }

    #endregion

    #region Target Methods

    protected GameObject FindClosestTarget()
    {
        GameObject[] turrets = FindGameObjectsWithTags("PlayerTurret","PlayerTower");
        GameObject currentTarget = null;
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (var target in turrets)
        {
            if(target.activeSelf==false) continue;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        currentTarget = closestTarget;
        return currentTarget;
    }

    GameObject[] FindGameObjectsWithTags(params string[] tags)
    {
        var all = new List<GameObject>();
        foreach (string tag in tags)
        {
            all.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }
        return all.ToArray();
    }   
        
    #endregion
}
