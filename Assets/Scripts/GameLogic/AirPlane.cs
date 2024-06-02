using System;
using System.Collections.Generic;
using UnityEngine;

public class AirPlane : MonoBehaviour, IPauseListener
{
    private const float TRACE_POINTS_DISTANCE = 10f;

    public event Action<AirPlane, Collider> OnCollision;

    [SerializeField] private float _minSpeed = 3f;
    [SerializeField] private float _defaultSpeed = 10f;
    [SerializeField] private float _maxSpeed = 20f;
    [SerializeField] private float _rolledPitchMaxAngularSpeed = 10f;

    [SerializeField] private AnimationCurve _autoPilotCurve;

    [SerializeField] private float _pitchMaxAngularSpeedDegrees = 20f;
    [SerializeField] private float _rollMaxAngularSpeedDegrees = 90f;

    private readonly List<Vector3> _trail = new List<Vector3>();
    private Vector3 _lastTrailPoint;

    public float Speed => _currentSpeed;
    public float MinSpeed => _minSpeed;
    public float DefaultSpeed => _defaultSpeed;
    public float MaxSpeed => _maxSpeed;
    public float Height => transform.position.y;
    public float Yaw
    {
        get 
        {
            Vector3 horizontalForward = GetHorizontal(transform.forward);
            float angle = Vector3.Angle(horizontalForward, Vector3.forward);
            return transform.forward.x >= 0 ? angle : 360 - angle;
        }
    }

    public float Pitch
    {
        get
        {
            Vector3 horizontalForward = GetHorizontal(transform.forward);
            float angle = Vector3.Angle(horizontalForward, transform.forward);
            return transform.forward.y <= 0 ? angle : 360 - angle;
        }
    }

    public float Roll
    {
        get
        {
            Vector3 horizontalRight = GetHorizontal(transform.right);
            float angle = Vector3.Angle(horizontalRight, transform.right);
            return transform.right.y >= 0 ? angle : 360 - angle;
        }
    }

    private Rigidbody _rigidBody;

    private Vector2 _steeringWheelPosition;
    private bool _isAutoPilotEnabled;
    private float _currentSpeed;
    private bool _isPaused;

    public void OnPause()
    {
        _isPaused = true;
    }

    public void OnResume()
    {
        _isPaused = false;
    }

    public void SetSteeringWheel(Vector2 value)
    {
        _steeringWheelPosition = value;
    }

    public void EnableAutoPilot()
    {
        _isAutoPilotEnabled = true;
    }

    public void DisableAutoPilot()
    {
        _isAutoPilotEnabled = false;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _currentSpeed = _defaultSpeed;
        _lastTrailPoint = transform.position;
    }

    public void Update()
    {
        if (_isPaused)
        {
            return;
        }

        float rollAngleRad = GetAngleToHorizon(transform.right) * Mathf.Deg2Rad;
        float sideVectorDirection = -transform.right.y;
        float sinAngleToHorizon = Mathf.Sin(rollAngleRad);
        Vector3 sideVector = sinAngleToHorizon * GetHorizontal(transform.right) * sideVectorDirection;

        Vector3 vertVector = (1f - transform.up.y) * Vector3.down * 0.3f;

        _currentSpeed = CalculateCurrentSpeed();

        _rigidBody.velocity = ((transform.forward + sideVector).normalized + vertVector).normalized * _currentSpeed;

        Vector3 angularVelocityFromRoll = Vector3.down * Mathf.Abs(transform.right.y) * Mathf.Sign(transform.right.y/* * transform.up.y*/) * _rolledPitchMaxAngularSpeed * Mathf.Deg2Rad;

        Vector3 localAngularVelocity = GetAngularVelocityFromSteeringWheel(GetSteeringWheelPosition());
        _rigidBody.angularVelocity = (transform.TransformDirection(localAngularVelocity) + angularVelocityFromRoll) * (_currentSpeed / _defaultSpeed);
        
        HandleTrail();
    }

    private void HandleTrail()
    {
        Vector3 currentPosition = transform.position;
        float distanceToLastTrailPoint = Vector3.Distance(currentPosition, _lastTrailPoint);
        if (distanceToLastTrailPoint >= TRACE_POINTS_DISTANCE)
        {
            _lastTrailPoint = currentPosition;
            _trail.Add(currentPosition);
        }
    }

    private Vector2 GetSteeringWheelPosition()
    {
        return _isAutoPilotEnabled ? GetAutoPilotSteeringWheel() : _steeringWheelPosition;
    }

    private float CalculateCurrentSpeed()
    {
        float currentMaxSpeed = (transform.forward.y < 0) ? Mathf.Lerp(_defaultSpeed, _maxSpeed, -transform.forward.y) : Mathf.Lerp(_defaultSpeed, _minSpeed, transform.forward.y);
        return Mathf.MoveTowards(_currentSpeed, currentMaxSpeed, _currentSpeed / currentMaxSpeed);
    }

    private float GetAxisPower(Vector3 direction, float minAngleOfMaxPower)
    {
        float angleToHorizon = GetAngleToHorizon(direction);
        float tempPower = Mathf.Min(1f, angleToHorizon / minAngleOfMaxPower);
        return _autoPilotCurve.Evaluate(tempPower);
    }

    private Vector3 GetAngularVelocityFromSteeringWheel(Vector2 steeringWheel)
    {
        float pitchVelocity = steeringWheel.y * _pitchMaxAngularSpeedDegrees * Mathf.Deg2Rad;
        float rollVelocity = -steeringWheel.x * _rollMaxAngularSpeedDegrees * Mathf.Deg2Rad;
        return new Vector3(pitchVelocity, 0, rollVelocity);
    }

    private Vector2 GetAutoPilotSteeringWheel()
    {
        float rollPower = GetAxisPower(transform.right, 45f);
        float rollDirection = Mathf.Sign(transform.right.y);
        float pitchPower = GetAxisPower(transform.forward, 45f);
        float pitchDirection = Mathf.Sign(transform.up.y * transform.forward.y);
        return new Vector2(rollPower * rollDirection, pitchPower * pitchDirection);
    }

    private Vector3 GetHorizontal(Vector3 direction)
    {
        return new Vector3(direction.x, 0f, direction.z);
    }
    private float GetAngleToHorizon(Vector3 direction)
    {
        Vector3 horizontalDirection = GetHorizontal(direction);
        return Vector3.Angle(direction, horizontalDirection);
    }

    private void OnDrawGizmos()
    {
        if (_trail.Count == 0)
            return;

        Gizmos.color = Color.white;
        for (int i = 1; i < _trail.Count; i++) 
        {
            Gizmos.DrawLine(_trail[i - 1], _trail[i]);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(this, collision.collider);
    }
}