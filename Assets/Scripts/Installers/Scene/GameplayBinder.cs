using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayBinder : MonoBehaviour
{
    [SerializeField] private TriggerZone[] _endLevelTriggers;

    [Inject] private GameplayState _gameplayState;
    [Inject] private GameplayUIHandler _gameplayUIHandler;
    [Inject] private AirPlane _plane;
    [Inject] private IRallyPointsChain _rallyPointsChain;
    [Inject] private TimeRecordsHandler _timeRecordsHandler;

    private void OnEnable()
    {
        BindAll();
    }

    private void BindAll()
    {
        _gameplayState.OnExit += UnbindAll;

        _rallyPointsChain.OnRallyPointReached += _gameplayState.OnRallyPointReached;

        _rallyPointsChain.OnRallyPointChanged += _gameplayState.OnRallyPointChanged;

        _gameplayUIHandler.SetMaxScore(_rallyPointsChain.RallyPointsCount);

        _gameplayState.OnScoreChanged += _gameplayUIHandler.OnScoreChanged;
        _gameplayState.OnTargetChanged += _gameplayUIHandler.SetTarget;

        _gameplayState.OnStartGame += _timeRecordsHandler.StartTimer;
        _gameplayState.OnLoseGame += _timeRecordsHandler.Reset;
        _gameplayState.OnWinGame += _timeRecordsHandler.FinishTimer;
        _gameplayState.OnPause += _timeRecordsHandler.Pause;
        _gameplayState.OnResume += _timeRecordsHandler.Resume;
        _gameplayState.OnPause += _gameplayUIHandler.OnPause;
        _gameplayState.OnResume += _gameplayUIHandler.OnResume;

        _gameplayState.OnLoseGame += _gameplayUIHandler.OpenLosePanel;
        _gameplayState.OnWinGame += _gameplayUIHandler.OpenWinPanel;

        BindPlane();

        foreach (TriggerZone trigger in _endLevelTriggers)
        {
            trigger.OnCollide += _gameplayState.OnEndLevelTriggered;
        }

        _gameplayUIHandler.OnMenuSessionStarted += _gameplayState.Pause;
        _gameplayUIHandler.OnMenuSessionFinished += _gameplayState.Resume;

        if (_rallyPointsChain.HasActivePoint())
        {
            _gameplayUIHandler.SetTarget(_rallyPointsChain.CurrentRallyPoint.transform);
            _gameplayUIHandler.SetMaxScore(_rallyPointsChain.RallyPointsCount);
        }
    }

    private void UnbindAll()
    {
        _gameplayState.OnExit -= UnbindAll;

        _rallyPointsChain.OnRallyPointReached -= _gameplayState.OnRallyPointReached;

        _rallyPointsChain.OnRallyPointChanged -= _gameplayState.OnRallyPointChanged;

        _gameplayState.OnScoreChanged -= _gameplayUIHandler.OnScoreChanged;
        _gameplayState.OnTargetChanged -= _gameplayUIHandler.SetTarget;

        _timeRecordsHandler.Reset();

        _gameplayState.OnStartGame -= _timeRecordsHandler.StartTimer;
        _gameplayState.OnLoseGame -= _timeRecordsHandler.Reset;
        _gameplayState.OnWinGame -= _timeRecordsHandler.FinishTimer;
        _gameplayState.OnPause -= _timeRecordsHandler.Pause;
        _gameplayState.OnResume -= _timeRecordsHandler.Resume;

        _gameplayState.OnPause += _gameplayUIHandler.OnPause;
        _gameplayState.OnResume += _gameplayUIHandler.OnResume;

        _gameplayState.OnLoseGame -= _gameplayUIHandler.OpenLosePanel;
        _gameplayState.OnWinGame -= _gameplayUIHandler.OpenWinPanel;

        UnbindPlane();

        foreach (TriggerZone trigger in _endLevelTriggers)
        {
            trigger.OnCollide -= _gameplayState.OnEndLevelTriggered;
        }

        _gameplayUIHandler.OnMenuSessionStarted -= _gameplayState.Pause;
        _gameplayUIHandler.OnMenuSessionFinished -= _gameplayState.Resume;
    }

    private void BindPlane()
    {
        _gameplayState.OnPause += _plane.OnPause;
        _gameplayState.OnResume += _plane.OnResume;

        if (_plane.TryGetComponent(out PlaneEngineSoundController soundController))
        {
            _gameplayState.OnWinGame += soundController.PauseSound;
            _gameplayState.OnPause += soundController.PauseSound;
            _gameplayState.OnResume += soundController.ResumeSound;
        }

        _plane.OnCollision += _gameplayState.OnPlaneCollision;
    }

    private void UnbindPlane()
    {
        _gameplayState.OnPause -= _plane.OnPause;
        _gameplayState.OnResume -= _plane.OnResume;

        if (_plane.TryGetComponent(out PlaneEngineSoundController soundController))
        {
            _gameplayState.OnWinGame -= soundController.PauseSound;
            _gameplayState.OnPause -= soundController.PauseSound;
            _gameplayState.OnResume -= soundController.ResumeSound;
        }

        _plane.OnCollision -= _gameplayState.OnPlaneCollision;
    }

    private void OnValidate()
    {
        List<TriggerZone> validTriggers = new List<TriggerZone>();

        for (int i = 0; i < _endLevelTriggers.Length; i++) 
        {
            if (_endLevelTriggers[i] != null)
            {
                validTriggers.Add(_endLevelTriggers[i]);
            }
        }
        _endLevelTriggers = validTriggers.ToArray();
    }
}
