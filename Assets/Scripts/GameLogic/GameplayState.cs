using System;
using UnityEngine;
using Zenject;

public class GameplayState : GameState
{
    private const float RESULT_SOUND_DELAY = 0.5f;

    public event Action<int> OnScoreChanged;
    public event Action<Transform> OnTargetChanged;

    private int _score;
    private GameObject _explosionPrefab;
    private string _sceneName;

    [Inject] private SoundEffectsSource _soundEffectsSource;

    public GameplayState(FadeEffect fadeEffect, GameObject explosionPrefab, string sceneName) : base(fadeEffect)
    {
        _explosionPrefab = explosionPrefab;
        _sceneName = sceneName;
    }

    public void OnRallyPointReached(IRallyPointsChain rallyPointsChain, RallyPoint point)
    {
        _soundEffectsSource.PlayCheckPointSound();

        if (rallyPointsChain.HasNextPoint())
        {
            rallyPointsChain.ActivateNextPoint();
            OnScoreChanged?.Invoke(++_score);
        }
        else
        {
            rallyPointsChain.DisableAllPoints();
            _soundEffectsSource.PlayWinSound(RESULT_SOUND_DELAY);
            OnTargetChanged?.Invoke(null);
        }
    }

    public void OnRallyPointChanged(IRallyPointsChain rallyPointsChain, RallyPoint point)
    {
        OnTargetChanged?.Invoke(point.transform);
    }

    public void OnPlaneCollision(AirPlane plane, Collider collider)
    {
        plane.gameObject.SetActive(false);
        _soundEffectsSource.PlayLoseSound(RESULT_SOUND_DELAY);
        UnityEngine.Object.Instantiate(_explosionPrefab, plane.transform.position, Quaternion.identity);
    }

    protected override string GetSceneName() => _sceneName;

    protected override void OnEnter()
    {
        base.OnEnter();
        _score = 0;
    }
}