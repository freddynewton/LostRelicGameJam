using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float stoppingDistance;

    [Header("Wandering Settings")]
    [SerializeField] private float movementWanderingRange;
    [SerializeField] private float movementWanderingDistanceMinimum;
    [SerializeField] private Vector2 movementWanderingWaitTime;
    private bool canWandering = true;

    [Header("Assigns")]
    [SerializeField] private Rigidbody rb;

    private Vector3 wanderingGoalPoint;
    private NavMeshPath path;
    private List<Vector3> pathCorners;

    public void ApplyPullForce(Vector3 targetPosition, float pullForceStregth)
    {
        rb.MovePosition(transform.position + (targetPosition - transform.position).normalized * pullForceStregth);
        //rb.AddForce((player.transform.position - transform.position).normalized * pullForceStregth, ForceMode.Force);
    }

    private void Update()
    {
        ApplyWandering();
    }

    private void ApplyWandering()
    {
        if ((wanderingGoalPoint == null || path == null || pathCorners.Count == 0 || Vector3.Distance(wanderingGoalPoint, transform.position) < stoppingDistance) && canWandering)
        {
            canWandering = false;
            Invoke("ActivateWandering", Random.Range(movementWanderingWaitTime.x, movementWanderingWaitTime.y));
            GetNavigationPath();
            wanderingGoalPoint = GetRandomWanderingPoint();
        }

        if (canWandering && pathCorners.Count > 0)
        {
            if (Vector3.Distance(pathCorners[0], transform.position) < stoppingDistance)
            {
                pathCorners.RemoveAt(0);
            }
            else
            {
                ApplyPullForce(pathCorners[0], movementSpeed);
            }
        }
    }

    private void ActivateWandering()
    {
        canWandering = true;
    }

    private Vector3 GetRandomWanderingPoint()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(Random.insideUnitSphere * movementWanderingRange, out hit, 100, -1);

        while (Vector3.Distance(transform.position, hit.position) < movementWanderingDistanceMinimum)
        {
            NavMesh.SamplePosition(Random.insideUnitSphere * movementWanderingRange, out hit, 100, -1);
        }

        return hit.position;
    }

    private void GetNavigationPath()
    {
        if (path == null)
        {
            path = new NavMeshPath();
        }

        NavMesh.CalculatePath(transform.position, wanderingGoalPoint, -1, path);
        pathCorners = path.corners.ToList();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.1f);
        Gizmos.DrawSphere(transform.position, movementWanderingRange);

        if (wanderingGoalPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wanderingGoalPoint, 0.5f);
        }

        if (path != null)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < pathCorners.Count - 1; i++)
            {
                Gizmos.DrawLine(pathCorners[i], pathCorners[i + 1]);
            }
        }
    }
}
