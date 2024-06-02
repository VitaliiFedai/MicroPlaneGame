using System;
using System.Collections.Generic;
using UnityEngine;

public class RallyPointSystem : IRallyPointsHolder, IRallyPointsChain, IRallyPointsGizmosDrawer
{
    public event Action<IRallyPointsChain, RallyPoint> OnRallyPointReached;
    public event Action<IRallyPointsChain, RallyPoint> OnRallyPointChanged;
    public event Action<IRallyPointsChain, RallyPoint> OnRallyPointsEnabled;
    public event Action<IRallyPointsChain> OnRallyPointsDisabled;
    public int RallyPointsCount => _rallyPoints.Count;

    private readonly List<RallyPoint> _rallyPoints = new List<RallyPoint>();
    private int _currentRallyPointIndex;
    public RallyPoint CurrentRallyPoint => _currentRallyPointIndex >= 0 ? _rallyPoints[_currentRallyPointIndex] : null;

    public void SetRallyPoints(IEnumerable<RallyPoint> rallyPoints)
    {
        ClearRallyPoints();
        _rallyPoints.AddRange(rallyPoints);
        BindRallyPointsEvents();
        DisableAllPoints();
        if (HasNextPoint())
        {
            ActivateNextPoint();
            SetAllDistances();
        }
    }

    public void ClearRallyPoints()
    {
        UnbindRallyPointsEvents();
        _rallyPoints.Clear();
        _currentRallyPointIndex = -1;
    }

    public bool HasNextPoint()
    { 
        return _currentRallyPointIndex + 1 < RallyPointsCount;
    }

    public bool HasActivePoint()
    { 
        return _currentRallyPointIndex >= 0;
    }

    public void ActivateNextPoint()
    {
        if (_currentRallyPointIndex >= 0)
        {
            DisablePoint(_currentRallyPointIndex);
        }
        _currentRallyPointIndex++;
        EnablePoint(_currentRallyPointIndex);
        OnRallyPointChanged?.Invoke(this, CurrentRallyPoint);
    }

    public void DisableAllPoints()
    {
        foreach (RallyPoint point in _rallyPoints)
        {
            DisablePoint(point);
        }
        OnRallyPointsDisabled?.Invoke(this);
    }

    public void EnableActivePoint()
    {
        if (_currentRallyPointIndex >= 0)
        { 
            EnablePoint(_currentRallyPointIndex);
        }
    }

    public void DrawGizmos()
    {
        if (_rallyPoints != null && RallyPointsCount > 0)
        {
            Gizmos.color = Color.yellow;
            Vector3 pointPositionA = _rallyPoints[0].transform.position;
            for (int index = 1; index < RallyPointsCount; index++)
            {
                RallyPoint point = _rallyPoints[index];
                Vector3 pointPositionB = point.transform.position;
                Gizmos.DrawLine(pointPositionA, pointPositionB);
                pointPositionA = pointPositionB;
            }
        }
    }

    private void EnablePoint(int pointIndex)
    { 
        SetPointEnable(pointIndex,true);
    }

    private void DisablePoint(int pointIndex)
    { 
        SetPointEnable(pointIndex, false);
    }

    private void EnablePoint(RallyPoint point)
    { 
        SetPointEnable(point,true);
    }

    private void DisablePoint(RallyPoint point)
    { 
        SetPointEnable(point, false);
    }

    private void SetPointEnable(int pointIndex, bool active)
    {
        SetPointEnable(_rallyPoints[pointIndex], active);
    }

    private void SetPointEnable(RallyPoint point, bool active)
    {
        point.gameObject.SetActive(active);
        if (active)
        {
            OnRallyPointsEnabled?.Invoke(this, point);
        }
    }

    private void BindRallyPointsEvents()
    {
        foreach (RallyPoint rallyPoint in _rallyPoints)
        {
            rallyPoint.OnCollide += OnCollidePerformed;
        }
    }

    private void UnbindRallyPointsEvents()
    {
        foreach (RallyPoint rallyPoint in _rallyPoints)
        {
            rallyPoint.OnCollide -= OnCollidePerformed;
        }
    }

    private void OnCollidePerformed(RallyPoint rallyPoint, AirPlane plane)
    {
        OnRallyPointReached?.Invoke(this, rallyPoint);
    }

    private void SetAllDistances()
    {
        if (_rallyPoints.Count == 0)
        {
            return;
        }

        RallyPoint previousPoint = _rallyPoints[0];
        for (int i = 1; i < _rallyPoints.Count; i++)
        { 
            RallyPoint point = _rallyPoints[i];
            int distance = (int)Vector3.Distance(previousPoint.transform.position, point.transform.position);
        }
    }
}