using CustomUI;
using HelpUtils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuPanel : CustomUIPanel
{
    [SerializeField] private CustomUIPanel _settingsPanelPrefab;

    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [Inject] private PlaneGameStateMachine _gameStateMachine;

    protected override void RegisterEvents(IUIEventsRegistrator registrator)
    {
        registrator.RegisterButtonClickEvent(_continueButton, CloseRequest);
        registrator.RegisterButtonClickEvent(_restartButton, OnRestartButtonClicked);
        registrator.RegisterButtonClickEvent(_settingsButton, OnSettingsButtonClicked);
        registrator.RegisterButtonClickEvent(_exitButton, OnExitButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.Gameplay);
    }

    private void OnSettingsButtonClicked()
    {
        OpenPanelRequest(_settingsPanelPrefab);
    }

    private void OnExitButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.MainMenu);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Validator.ValidatePrefab(ref _settingsPanelPrefab, nameof(_settingsPanelPrefab));
    }
    #endif
}