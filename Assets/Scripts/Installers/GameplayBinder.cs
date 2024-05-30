using UnityEngine;
using Zenject;

public class GameplayBinder : MonoBehaviour
{
    [Inject] private GameplayState _gameplayState;
    [Inject] private GameplayUIHandler _gameplayUIHandler;
    [Inject] private AirPlane _plane;
    [Inject] private IRallyPointsChain _rallyPointsChain;

    private void OnEnable()
    {
        _rallyPointsChain.OnRallyPointReached += _gameplayState.OnRallyPointReached;
        _rallyPointsChain.OnRallyPointChanged += _gameplayState.OnRallyPointChanged;
        _gameplayState.OnScoreChanged += _gameplayUIHandler.OnScoreChanged;
        _gameplayState.OnTargetChanged += _gameplayUIHandler.SetTarget;
        _gameplayUIHandler.SetTarget(_rallyPointsChain.CurrentRallyPoint.transform);
        _plane.OnCollision += _gameplayState.OnPlaneCollision;
    }

    private void OnDisable()
    {
        _rallyPointsChain.OnRallyPointReached -= _gameplayState.OnRallyPointReached;
        _rallyPointsChain.OnRallyPointChanged -= _gameplayState.OnRallyPointChanged;
        _gameplayState.OnScoreChanged -= _gameplayUIHandler.OnScoreChanged;
        _gameplayState.OnTargetChanged -= _gameplayUIHandler.SetTarget;
        _plane.OnCollision -= _gameplayState.OnPlaneCollision;
    }
}
