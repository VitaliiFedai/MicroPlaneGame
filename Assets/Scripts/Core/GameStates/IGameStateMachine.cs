public interface IGameStateMachine
{
    public void Initialize(IGameState startState);
    public void ChangeState(IGameState newState);
}
