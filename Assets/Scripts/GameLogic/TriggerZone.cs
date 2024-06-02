using System;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public event Action<TriggerZone, AirPlane> OnCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AirPlane airplane))
        {
            OnCollide?.Invoke(this, airplane);
        }
    }
}
