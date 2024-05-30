using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Controller : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [Inject] private PlaneGameStateMachine _gameStateMachine;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        _gameStateMachine.ChangeState(_gameStateMachine.MainMenu);
    }
}
