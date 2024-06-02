using System;
using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetTimeSeconds(float time)
    { 
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _text.text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    private void Reset()
    {
        if (TryGetComponent(out TMP_Text text))
        {
            _text = text;
        }
    }
}
