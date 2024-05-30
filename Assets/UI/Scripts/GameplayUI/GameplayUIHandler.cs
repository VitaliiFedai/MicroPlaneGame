using CustomUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


[DisallowMultipleComponent]
public class GameplayUIHandler : UIHandler
{
    [SerializeField] private Transform _parentPanel;
    [SerializeField] private VirtualJoystick _virtualJoystick;

    [SerializeField] private TMP_Text _speedValueText;
    [SerializeField] private TMP_Text _heightValueText;
    [SerializeField] private TMP_Text _yawValueText;
    [SerializeField] private TMP_Text _pitchValueText;
    [SerializeField] private TMP_Text _rollValueText;
    [SerializeField] private TMP_Text _scoreValueText;
    [SerializeField] private TMP_Text _fpsValueText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Transform _targetDirectionArrow;
    
    [Inject] private AirPlane _plane;
    [Inject] private PlaneGameStateMachine _gameStateMachine;
    [Inject] private SoundEffectsSource _soundEffectsSource;

    private Transform _target;

    private UIEventsBinder _eventsBinder;

    public void OnScoreChanged(int newScore)
    {
        _scoreValueText.text = newScore.ToString();
    }

    public void SetTargetDirectionAngle(float angle)
    {
        _targetDirectionArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    protected override Transform GetParentPanel() => _parentPanel;

    protected override void OnEnterScene()
    {
        _virtualJoystick.OnChanged += _plane.SetSteeringWheel;
        _virtualJoystick.OnPressed += _plane.DisableAutoPilot;
        _virtualJoystick.OnReleased += _plane.EnableAutoPilot;
        _eventsBinder.BindListeners();
    }

    protected override void OnExitScene()
    {
        _eventsBinder.UnbindListeners();
        _virtualJoystick.OnChanged -= _plane.SetSteeringWheel;
        _virtualJoystick.OnPressed -= _plane.DisableAutoPilot;
        _virtualJoystick.OnReleased -= _plane.EnableAutoPilot;
    }

    protected override void OnClick()
    {
        _soundEffectsSource.PlayClick();
    }

    protected override void OnToggleChanged(bool value)
    {
        OnClick();
    }

    private void Awake()
    {
        _eventsBinder = new UIEventsBinder(OnClick, OnToggleChanged);
        _eventsBinder.RegisterButtonClickEvent(_restartButton, OnRestartButtonClicked);
        _eventsBinder.RegisterButtonClickEvent(_exitButton, OnExitButtonClicked);
    }

    private void Update()
    {
        _scoreValueText.text = $"{Camera.main.pixelWidth} : {Camera.main.pixelHeight}";

        _speedValueText.text = _plane.Speed.ToString();
        _heightValueText.text = _plane.Height.ToString();
        _yawValueText.text = ((int)_plane.Yaw).ToString();
        _pitchValueText.text = ((int)_plane.Pitch).ToString();
        _rollValueText.text = ((int)_plane.Roll).ToString();
        _fpsValueText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();

        if (_target != null)
        {
            UpdateArrow();
        }
    }

    private void UpdateArrow()
    {
        Vector3 vectorToTarget = (_target.position - _plane.transform.position);
        Ray ray = new Ray(_plane.transform.position, vectorToTarget);
        
        // [0] - Left, [1] - Right, [2] - Down, [3] - Up
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        float minDistance = Mathf.Infinity;
        for (int i = 0; i < 4; i++)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }

        minDistance = Mathf.Clamp(minDistance, 0, vectorToTarget.magnitude);

        bool showTargetArrow = minDistance < vectorToTarget.magnitude;

        _targetDirectionArrow.gameObject.SetActive(showTargetArrow);
        if (showTargetArrow)
        {
            Vector3 worldPOsition = ray.GetPoint(minDistance);

            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPOsition);

            Vector2 centerPosition = new Vector2(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
            Vector2 screenPositionFromCenter = screenPosition - centerPosition;

            float angle = Vector3.Angle(screenPositionFromCenter, Vector2.up);
            if (screenPositionFromCenter.x > 0)
            {
                angle = -angle;
            }

            SetTargetDirectionAngle(angle);
        }    
    }

    private void Reset()
    {
        if (_parentPanel == null)
        { 
            Canvas camvas = FindObjectOfType<Canvas>();
            _parentPanel = camvas?.transform;
        }
    }

    private void OnRestartButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.Gameplay);
    }

    private void OnExitButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.MainMenu);
    }
}