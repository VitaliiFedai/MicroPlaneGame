using System;

public interface IRallyPointsChain
{
    public event Action<IRallyPointsChain, RallyPoint> OnRallyPointReached;
    public event Action<IRallyPointsChain, RallyPoint> OnRallyPointChanged;
    public event Action<IRallyPointsChain> OnRallyPointsDisabled;
    public event Action<IRallyPointsChain, RallyPoint> OnRallyPointsEnabled;

    public RallyPoint CurrentRallyPoint { get; }
    public int RallyPointsCount { get; }
    public bool HasNextPoint();
    public bool HasActivePoint();
    public void ActivateNextPoint();
    public void DisableAllPoints();
    public void EnableActivePoint();
}
