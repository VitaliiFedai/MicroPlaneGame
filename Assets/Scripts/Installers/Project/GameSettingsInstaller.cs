using Zenject;

public class GameSettingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameSettings>().AsSingle().NonLazy();
    }
}