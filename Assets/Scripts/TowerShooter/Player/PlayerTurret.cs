using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float shootInterval = 1f;
    private GameObject turretMesh;

    private List<Transform> targets = new();

    private Transform currentTarget = null;

    void Start()
    {
        turretMesh = transform.GetChild(0).gameObject;
    }

    private void FindAllTargets()
    {
        targets.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            targets.Add(enemy.transform);
        }
    }

    private void FindTheClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (var target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        currentTarget = closestTarget;
    }

    void Update()
    {
        FindAllTargets();
        FindTheClosestTarget();

        if (currentTarget != null)
        {
            turretMesh.transform.LookAt(currentTarget);
        }
    }
}
