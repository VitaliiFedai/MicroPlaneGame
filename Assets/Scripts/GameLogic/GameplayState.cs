using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GameplayState : GameState
{
    private const int RESULT_SOUND_DELAY_MILISECONDS = 500;

    public event Action<int> OnScoreChanged;
    public event Action<Transform> OnTargetChanged;
    public event Action OnWinGame;
    public event Action OnLoseGame;
    public event Action OnStartGame;

    public event Action OnPause;
    public event Action OnResume;

    private int _score;
    private GameObject _explosionPrefab;
    private string _sceneName;

    private bool _isPaused;

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
            OnTargetChanged?.Invoke(null);
        }
    }

    public void OnRallyPointChanged(IRallyPointsChain rallyPointsChain, RallyPoint point)
    {
        OnTargetChanged?.Invoke(point.transform);
    }

    public async void OnPlaneCollision(AirPlane plane, Collider collider)
    {
        plane.gameObject.SetActive(false);
        UnityEngine.Object.Instantiate(_explosionPrefab, plane.transform.position, Quaternion.identity);

        await Task.Delay(RESULT_SOUND_DELAY_MILISECONDS);
        _soundEffectsSource.PlayLoseSound();
        OnLoseGame?.Invoke();
    }

    public void Pause()
    {
        if (_isPaused)
        {
            throw new InvalidOperationException("Can''t Pause Game because it is paused already!");
        }

        _isPaused = true;

        DisablePhysics();
        OnPause?.Invoke();
    }

    public void Resume()
    {
        if (!_isPaused)
        {
            throw new InvalidOperationException("Can''t Resume Game because it was not paused!");
        }

        _isPaused = false;
        EnablePhysics();
        OnResume?.Invoke();
    }

    public void OnEndLevelTriggered(TriggerZone zone, AirPlane plane)
    {
        OnScoreChanged?.Invoke(++_score);
        _soundEffectsSource.PlayWinSound();
        OnWinGame?.Invoke();
        FinishGame();
    }

    protected override string GetSceneName() => _sceneName;

    protected override void OnLoadSceneFinished()
    {
        StartGame();    
    }

    private void StartGame()
    {
        _score = 0;
        _isPaused = false;
        EnablePhysics();
        OnStartGame?.Invoke();
    }

    private void FinishGame()
    {
        DisablePhysics();
    }

    private void DisablePhysics()
    {
        Physics.simulationMode = SimulationMode.Script;
    }

    private void EnablePhysics()
    {
        Physics.simulationMode = SimulationMode.FixedUpdate;
    }
}
