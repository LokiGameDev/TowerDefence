using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarbaneMoth : Enemy
{
     private GameObject _currentTarget;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int enemyValue;
    [SerializeField]
    private float enemyHealth;
    [SerializeField]
    private float attackRange, shootInterval;
    private bool canShoot;

    void OnEnable()
    {
        base._enemyHealth = enemyHealth;
        base.localMaxHealth = enemyHealth;
        base._enemyValue = enemyValue;
        base.isAlive = true;
        base.isStunned = false;
        canShoot = true;
        _currentTarget = null;
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
        if(Vector3.Distance(transform.position, targetPosition) < attackRange)
        {
            enemyAnimation.EnemyAnimationTrigger("Attack");
            if (canShoot)
            {
                DealAreaDamage();
                canShoot = false;
                if(this.gameObject.activeSelf) StartCoroutine(ShootCooldown());
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(targetPosition); 
        }
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }

    public float radius = 7f;
    public LayerMask enemyMask;

    public void DealAreaDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyMask);
        PlayCircleEffect();

        foreach (Collider col in hits)
        {
            IDamagable dmg = col.GetComponent<IDamagable>();
            dmg?.GotHit(5);
        }
    }

    #region Circle Visual
    public int segments = 60; // smoother = higher value
    public LineRenderer lineRenderer;

    public void PlayCircleEffect(float duration = 0.3f)
    {
        lineRenderer.enabled = true;

        // Reset alpha
        Color c = lineRenderer.startColor;
        c.a = 1f;
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;

        DrawCircle();
        StartCoroutine(FadeCircle(duration));
    }

    private void DrawCircle()
    {
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, 0, z) + transform.position);

            angle += 360f / segments;
        }
    }

    private IEnumerator FadeCircle(float time)
    {
        float t = 0f;
        Color start = lineRenderer.startColor;
        Color end = new Color(start.r, start.g, start.b, 0);

        while (t < time)
        {
            float normalized = t / time;
            Color c = Color.Lerp(start, end, normalized);
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
            t += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }
    #endregion
}
