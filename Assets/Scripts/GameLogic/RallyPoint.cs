using System;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    public event Action<RallyPoint, AirPlane> OnCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AirPlane plane))
        {
            OnCollide?.Invoke(this, plane);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 5f);
    }
}