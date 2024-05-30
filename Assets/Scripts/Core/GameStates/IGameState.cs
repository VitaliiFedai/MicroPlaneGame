using System.Threading.Tasks;

public interface IGameState
{
    public Task Enter();
    public Task Exit();
    public void Update();
}
