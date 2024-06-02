using System;
using UnityEngine;
using Zenject;

public class TimeRecordsHandler : ITimeProvider, ITimeRecordProvider, ITickable
{
    private enum State
    { 
        Idle,
        Started,
        Paused
    }

    public event Action<float> OnTimeChanged;
    public event Action<float> OnBestTimeChanged;

    public float BestTime { get; private set; } = float.PositiveInfinity;
    
    private float _startTime;
    private float _finishTime;
    private State _state;

    private float _startPauseTime;
    private float _pastPausedTime;

    private bool IsStarted => _state == State.Started;
    private bool IsPaused => _state == State.Paused;
    private bool IsIdle => _state == State.Idle;

    public void StartTimer()
    {
        Debug.Log($"{nameof(StartTimer)}");
        Assert(IsIdle, $"Can''t call {nameof(TimeRecordsHandler)}.{nameof(StartTimer)} because timer is already started!");
        Reset();
        _state = State.Started;
        _startTime = Time.time;
    }

    public void FinishTimer()
    {
        Debug.Log($"{nameof(FinishTimer)}");

        Assert(IsStarted, $"Can''t call {nameof(TimeRecordsHandler)}.{nameof(FinishTimer)} because timer was not started!");

        _state = State.Idle;
        _finishTime = Time.time;
        float elapsedTime = GetCurrentTime();
        OnTimeChanged?.Invoke(elapsedTime);

        if (elapsedTime < BestTime)
        { 
            BestTime = elapsedTime;
            OnBestTimeChanged?.Invoke(BestTime);
        }
    }

    public void Reset()
    {
        _state = State.Idle;
        _startTime = 0;
        _finishTime = 0;
        _pastPausedTime = 0;
        _startPauseTime = 0;
        OnTimeChanged?.Invoke(0);
    }

    public void Pause()
    {
        Debug.Log($"{nameof(Pause)}");

        Assert(IsStarted, $"Can''t call {nameof(TimeRecordsHandler)}.{nameof(Pause)} because timer was {(IsPaused ? " paused already!" : "not started!")}");
        _state = State.Paused;
        _startPauseTime = Time.time;
        OnTimeChanged?.Invoke(GetCurrentTime());
    }

    public void Resume()
    {
        Debug.Log($"{nameof(Resume)}");

        Assert(IsPaused, $"Can''t call {nameof(TimeRecordsHandler)}.{nameof(Resume)} because timer in not paused!");
        _state = State.Started;
        _pastPausedTime += GetTimeFromPause();
    }

    public float GetCurrentTime()
    {
        return _state switch
        {
            State.Started => GetTimeFromStart() - _pastPausedTime,
            State.Paused => GetTimeFromStart() - _pastPausedTime - GetTimeFromPause(),
            _ => _finishTime - _startTime - _pastPausedTime,
        };
    }

    private void Assert(bool expression, string message) 
    {
        if (!expression)
        {
            throw new InvalidOperationException(message);
        }
    }

    private float GetTimeFromStart() => Time.time - _startTime;

    private float GetTimeFromPause() => Time.time - _startPauseTime;

    public void Tick()
    {
        if (IsStarted)
        {
            OnTimeChanged?.Invoke(GetCurrentTime());
        }
    }
}