using System;
using UnityEngine;
using UnityEngine.UI;

public class TargetDirectionArrow : MonoBehaviour
{
    [SerializeField] private Image _arrowImage;

    private Transform _target;

    private Transform _owner;

    private bool _isPaused;

    public bool IsArrowVisible
    {
        get => _arrowImage.enabled;
        set => _arrowImage.enabled = value;
    }

    public void SetTarget(Transform target)
    { 
        IsArrowVisible = target != null;
        _target = target;
    }

    public void SetOwner(Transform owner)
    { 
        _owner = owner;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    private void Awake()
    {
        IsArrowVisible = false;
    }

    private void Update()
    {
        if (!IsReady())
        {
            Debug.LogError($"{nameof(_owner)} == null!");
            return;
        }

        if (_isPaused || _arrowImage.IsDestroyed())
        {
            return;
        }

        if (_target == null)
        {
            IsArrowVisible = false;
        }
        else
        { 
            Vector3 vectorToTarget = (_target.position - _owner.transform.position);
            Ray ray = new Ray(_owner.transform.position, vectorToTarget);

            float distanceToTarget = vectorToTarget.magnitude;

            float minDistance = GetRayCastMinDistance(ray, distanceToTarget);

            IsArrowVisible = minDistance < distanceToTarget;

            if (IsArrowVisible)
            {
                Vector3 worldPosition = ray.GetPoint(minDistance);
                float angle = GetTargetDirectionAngle(worldPosition);
                SetArrowRotationAngle(angle);
            }
        }
    }

    private bool IsReady()
    {
        return _owner != null;
    }

    private float GetRayCastMinDistance(Ray ray, float distanceToTarget)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main); // [0] - Left, [1] - Right, [2] - Down, [3] - Up

        float minDistance = Mathf.Infinity;
        for (int i = 0; i < 4; i++)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }
        return Mathf.Clamp(minDistance, 0, distanceToTarget);
    }

    private void SetArrowRotationAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private float GetTargetDirectionAngle(Vector3 rayCastPointWorldPosition)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(rayCastPointWorldPosition);

        Vector2 centerPosition = new Vector2(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
        Vector2 screenPositionFromCenter = screenPosition - centerPosition;

        float angle = Vector3.Angle(screenPositionFromCenter, Vector2.up);
        if (screenPositionFromCenter.x > 0)
        {
            angle = -angle;
        }
        return angle;
    }

    private void OnValidate()
    {
        if (_arrowImage == null)
        {
            Debug.LogError($"{nameof(_arrowImage)} is null!");
        }
    }
}