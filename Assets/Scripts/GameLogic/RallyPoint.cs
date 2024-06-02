using System;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    public event Action<RallyPoint, AirPlane> OnCollide;

    [SerializeField] private GameObject _interactableObject;

    private IInteractable _interactable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AirPlane plane))
        {
            if (_interactable != null)
            {
                _interactable.Interact();
            }
            OnCollide?.Invoke(this, plane);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    private void OnValidate()
    {
        if (_interactableObject != null && !_interactableObject.TryGetComponent(out _interactable))
        { 
            _interactableObject = null;
        }
    }
}