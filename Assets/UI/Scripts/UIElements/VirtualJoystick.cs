using System;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    private const float MAX_MOVE_RADIUS = 400f;
    private const float MAX_MOVE_RADIUS_SQR = MAX_MOVE_RADIUS * MAX_MOVE_RADIUS;
    private const float IGNORED_MOVE_RADIUS = 100;
    private const float RELEASED_MOVE_SPEED = 2000f;
    private const float DELTA = 0.001f;
    private const float DELTA_SQR = DELTA * DELTA;

    [SerializeField] private VirtualJoystickSlider _slider;

    public event Action OnPressed;
    
    public event Action OnReleased;

    public event Action<Vector2> OnChanged;

    public bool IsPressed { get; private set; }

    private Vector2 _offset;
    private Vector2 _previousMove;

    private void OnEnable()
    {
        _slider.OnPressed += OnPressedPerform;
        _slider.OnReleased += OnReleasedPerform;
    }

    private void OnDisable()
    {
        _slider.OnPressed -= OnPressedPerform;
        _slider.OnReleased -= OnReleasedPerform;
    }

    private void Update()
    {
        if (IsPressed)
        {
            HandlePressedMove();
        }
        else
        {
            HandleReleasedMove();
        }

        Vector2 resultMove = GetResultMove();
        if ((resultMove - _previousMove).sqrMagnitude >= DELTA_SQR)
        {
            _previousMove = resultMove;
            OnChanged?.Invoke(resultMove);
        }
    }

    private void OnPressedPerform(Vector2 offset)
    {
        IsPressed = true;
        _offset = offset;
        OnPressed?.Invoke();
    }

    private void OnReleasedPerform()
    {
        IsPressed = false;
        OnReleased?.Invoke();
    }

    private void HandlePressedMove()
    {
        Vector2 pointerPosition = Input.mousePosition;
        _slider.transform.position = pointerPosition - _offset;
        Vector2 direction = _slider.transform.localPosition.normalized;
        if (_slider.transform.localPosition.sqrMagnitude > MAX_MOVE_RADIUS_SQR)
        {
            _slider.transform.localPosition = direction * MAX_MOVE_RADIUS;
        }
    }

    private void HandleReleasedMove()
    {
        if (_slider.transform.localPosition.sqrMagnitude > DELTA_SQR)
        {
            _slider.transform.localPosition = Vector3.MoveTowards(_slider.transform.localPosition, Vector3.zero, RELEASED_MOVE_SPEED * Time.deltaTime);
        }
        else
        {
            _slider.transform.localPosition = Vector3.zero;
        }
    }

    private Vector2 GetResultMove()
    {
        float distance = Mathf.Max(0f, _slider.transform.localPosition.magnitude - IGNORED_MOVE_RADIUS);
        float distanceNormalized = Mathf.InverseLerp(0f, MAX_MOVE_RADIUS - IGNORED_MOVE_RADIUS, distance);
        Vector2 direction = _slider.transform.localPosition.normalized;
        Vector2 resultMove = distanceNormalized > DELTA ? direction * distanceNormalized : Vector2.zero;
        return resultMove;
    }
}
