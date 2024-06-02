using CustomUI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameResultPanel : CustomUIPanel
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    [Inject] private PlaneGameStateMachine _gameStateMachine;

    protected override void RegisterEvents(IUIEventsRegistrator registrator)
    {
        registrator.RegisterButtonClickEvent(_restartButton, OnRestartButtonClicked);
        registrator.RegisterButtonClickEvent(_exitButton, OnOkButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.Gameplay);
    }

    private void OnOkButtonClicked()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.MainMenu);
    }

    private void OnValidate()
    {
        if (_exitButton == null) 
        {
            Debug.LogError($"{nameof(GameResultPanel)}.{nameof(_exitButton)} is null!");
        }
        if (_restartButton == null) 
        {
            Debug.LogError($"{nameof(GameResultPanel)}.{nameof(_restartButton)} is null!");
        }
    }
}