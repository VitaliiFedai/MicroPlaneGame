using CustomUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class GameplayUIHandler : UIHandler
{
    [SerializeField] private GameResultPanel _winPanelPrefab; 
    [SerializeField] private GameResultPanel _losePanelPrefab; 
    [SerializeField] private MenuPanel _menuPanelPrefab; 
    [SerializeField] private Transform _parentPanel;
    [SerializeField] private VirtualJoystick _virtualJoystick;
    [SerializeField] private Button _menuButton;
    [SerializeField] private TargetDirectionArrow _targetDirectionArrow;
    [SerializeField] private TMP_Text _scoreValueText;
    [SerializeField] private TimerText _timerText;

    [Inject] private AirPlane _plane;
    [Inject] private SoundEffectsSource _soundEffectsSource;
    [Inject] private TimeRecordsHandler _timeRecordsHandler;
    [Inject] private GameSettings _gameSettings;

    public event Action OnMenuSessionStarted;
    public event Action OnMenuSessionFinished;

    private UIEventsBinder _eventsBinder;
    private int _maxScore;
    private int _score;

    public void OpenWinPanel()
    {
        OpenPanel(_winPanelPrefab);
    }

    public void OpenLosePanel()
    {
        OpenPanel(_losePanelPrefab);
    }

    public void OnScoreChanged(int newScore)
    {
        _score = newScore;
        _scoreValueText.text = $"{newScore} / {_maxScore}";
    }

    public void SetMaxScore(int maxScore)
    {
        _maxScore = maxScore;
        _scoreValueText.text = $"{_score} / {_maxScore}";
    }

    public void SetTarget(Transform target) => _targetDirectionArrow.SetTarget(target);

    protected override Transform GetParentPanel() => _parentPanel;

    protected override void OnClick() => _soundEffectsSource.PlayClick();

    protected override void OnToggleChanged(bool value) => OnClick();

    protected override void OnEnterScene()
    {
    }

    protected override void OnExitScene()
    {
        _gameSettings.OnInvertUpDownChange -= _virtualJoystick.SetInvertUpDown;
        _virtualJoystick.OnChanged -= _plane.SetSteeringWheel;
        _virtualJoystick.OnPressed -= _plane.DisableAutoPilot;
        _virtualJoystick.OnReleased -= _plane.EnableAutoPilot;
        _eventsBinder.UnbindListeners();
        _timeRecordsHandler.OnTimeChanged -= _timerText.SetTimeSeconds;
    }

    private void Awake()
    {
        _eventsBinder = new UIEventsBinder(OnClick, OnToggleChanged);
        _eventsBinder.RegisterButtonClickEvent(_menuButton, OnMenuButtonClicked);
        _targetDirectionArrow.SetOwner(_plane.transform);

        _virtualJoystick.SetInvertUpDown(_gameSettings.InvertUpDown);
        _gameSettings.OnInvertUpDownChange += _virtualJoystick.SetInvertUpDown;
        _virtualJoystick.OnChanged += _plane.SetSteeringWheel;
        _virtualJoystick.OnPressed += _plane.DisableAutoPilot;
        _virtualJoystick.OnReleased += _plane.EnableAutoPilot;
        _eventsBinder.BindListeners();

        _timeRecordsHandler.OnTimeChanged += _timerText.SetTimeSeconds;
    }

    private void OnMenuButtonClicked()
    {
        OnMenuSessionStarted?.Invoke();
        OpenPanel(_menuPanelPrefab, OnMenuSessionFinished);
    }

    private void Reset()
    {
        if (_parentPanel == null)
        {
            Canvas camvas = FindObjectOfType<Canvas>();
            _parentPanel = camvas?.transform;
        }
    }

    public void OnPause()
    {
        _targetDirectionArrow.Pause();
    }

    public void OnResume()
    {
        _targetDirectionArrow.Resume();
    }
}