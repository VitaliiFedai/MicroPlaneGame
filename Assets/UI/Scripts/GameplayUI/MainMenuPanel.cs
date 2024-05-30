using CustomUI;
using HelpUtils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuPanel : CustomUIPanel
{
    [SerializeField] private CustomUIPanel _settingsPanelPrefab;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [Inject] private PlaneGameStateMachine _gameStateMachine;

    protected override void RegisterEvents(IUIEventsRegistrator registrator)
    {
        registrator.RegisterButtonClickEvent(_startButton, OnStartButtonClicked);
        registrator.RegisterButtonClickEvent(_settingsButton, OnSettingsButtonClicked);
        registrator.RegisterButtonClickEvent(_exitButton, OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.Gameplay);
    }

    private void OnSettingsButtonClicked()
    {
        OpenPanelRequest(_settingsPanelPrefab);
    }

    private void OnExitButtonClicked()
    {
        Debug.Log($"Application.Quit");
        Application.Quit();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        Validator.ValidatePrefab(ref _settingsPanelPrefab, nameof(_settingsPanelPrefab));
    }
    #endif
}