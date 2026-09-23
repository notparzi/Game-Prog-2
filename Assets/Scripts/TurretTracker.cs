using UnityEngine;
using System.Collections.Generic;

public class TurretTracker : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;

    private List<Transform> targetQueue =
        new List<Transform>();


    void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;

        direction.y = 0f;
        direction.Normalize();

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp
        (
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        float aimAlignment = Vector3.Dot
        (
            transform.forward,
            direction
        );

        if (aimAlignment > 0.98f)
        {
            Debug.DrawLine(transform.position, target.position, Color.red);
        }
    }

    public void RegisterTarget(Transform newTarget)
    {
        if (targetQueue.Contains(newTarget))
        {
            return;
        }

        targetQueue.Add(newTarget);
        if (target == null)
        {
            SetNextTarget();
        }
    }

    public void UnregisterTarget(Transform leavingTarget)
    {
        if (targetQueue.Contains(leavingTarget))
        {
            targetQueue.Remove(leavingTarget);
        }

        if (target == leavingTarget)
        {
            target = null;

            SetNextTarget();
        }
    }

    void SetNextTarget()
    {
        targetQueue.RemoveAll(
            item => item == null
        );

        if (targetQueue.Count == 0)
        {
            target = null;
            return;
        }

        target = targetQueue[0];
    }
}