using Zenject;

public class PlaneGameStateMachine : GameStateMachine 
{
    public IGameState MainMenu => _mainMenuState;
    public IGameState Gameplay => _gameplayState;

    [Inject] private GameplayState _gameplayState;
    [Inject] private MainMenuState _mainMenuState;
}