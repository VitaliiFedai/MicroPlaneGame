using UnityEngine;
using Zenject;

public class ResultTimerPanel : MonoBehaviour
{
    [SerializeField] private TimerText _currentTimeText;
    [SerializeField] private Transform _bestTimePanel;
    [SerializeField] private TimerText _bestTimeText;

    [Inject] private TimeRecordsHandler _timeRecordsHandler;

    private void Awake()
    {
        _currentTimeText.SetTimeSeconds(_timeRecordsHandler.GetCurrentTime());
        _bestTimeText.SetTimeSeconds(_timeRecordsHandler.BestTime);
    }
}
