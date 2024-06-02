using System;

public interface ITimeProvider
{
    public event Action<float> OnTimeChanged;
    public float GetCurrentTime();
}