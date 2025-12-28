using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, ISpecialEffect
{
    #region Variables

    [SerializeField]
    protected EnemyAnimation enemyAnimation;
    public EnemyType enemyType;
    protected int _enemyValue;
    protected float _enemyHealth;
    protected float _enemyDamage;
    protected bool isAlive = false;
    protected bool isStunned = false;
    [SerializeField]
    protected Image healthImage;
    protected float localMaxHealth;

    public bool canMoveForTest;

    [SerializeField]
    protected GameObject deathEffect;

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        enemyAnimation.EnemyAnimationTrigger("Normal");
        localMaxHealth = _enemyHealth;
    }

    #endregion

    #region Enemy Methods

    public virtual void GotHit(float damage)
    {
        if(!isAlive) return;
        _enemyHealth-=damage;
        if(_enemyHealth > 0)
        {
            enemyAnimation.EnemyAnimationTrigger("Hit");
            Debug.Log($"Hit : Remaining life : {_enemyHealth}");
            healthImage.fillAmount = _enemyHealth/localMaxHealth;
            return;
        }
        else
        {
            isAlive = false;
            InitiateTheDeathEffect();
            healthImage.fillAmount = _enemyHealth/localMaxHealth;
            Debug.Log($"Dead : Remaining life : {_enemyHealth}");
            enemyAnimation.EnemyAnimationTrigger("Dead");
            GameManager.Instance.EnemyGotDestroyed();
            GameManager.Instance.AddScore(_enemyValue);
            StopAllCoroutines();
            StartCoroutine(ReturnToPoolDelay());
        }
    }

    protected void InitiateTheDeathEffect()
    {
        var deadEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        deadEffect.transform.position = transform.position;
    }

    public void Sacrificed()
    {
        isAlive = false;
        GameManager.Instance.EnemyGotDestroyed();
        StopAllCoroutines();
        EnemyPool.Instance.ReturnToPool(this);
    }

    public void Stunned()
    {
        Debug.Log("Stunned");
        isStunned = true;
        enemyAnimation.EnemyAnimationTrigger("Stunned");
        StartCoroutine(RemoveStunEFfect());
    }

    IEnumerator RemoveStunEFfect()
    {
        yield return new WaitForSeconds(5);
        isStunned = false;
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
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (var target in turrets)
        {
            if(target.activeSelf==false || !target.GetComponent<IDamagable>().isDamagable()) continue;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
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

    protected void SetEnemyHealthBar()
    {
        healthImage.fillAmount = _enemyHealth/localMaxHealth;
    }

    public bool isDamagable()
    {
        return isAlive;
    }

    #endregion
}
