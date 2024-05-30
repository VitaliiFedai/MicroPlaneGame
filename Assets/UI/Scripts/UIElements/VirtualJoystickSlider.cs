using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystickSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action<Vector2> OnPressed;
    public event Action OnReleased;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pointerOffset = new Vector2(eventData.position.x - transform.position.x, eventData.position.y - transform.position.y);
        OnPressed?.Invoke(pointerOffset);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleased?.Invoke();
    }
}
