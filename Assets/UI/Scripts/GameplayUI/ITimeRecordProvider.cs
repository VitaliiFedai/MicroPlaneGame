using System;

public interface ITimeRecordProvider
{
    public event Action<float> OnBestTimeChanged;
    public float BestTime { get; }
}