using UnityEngine;
using Zenject;

public class AppStartUp : MonoBehaviour
{
    [Inject] GameplayState _gameplayState;
    [Inject] PlaneGameStateMachine _gameStateMachine;

    private void Start()
    {
        Debug.Log($"{nameof(AppStartUp)}.{nameof(Start)}");
        _gameStateMachine.Initialize(_gameStateMachine.MainMenu);
    }
}