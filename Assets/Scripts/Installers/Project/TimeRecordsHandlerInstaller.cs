using Zenject;

public class TimeRecordsHandlerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TimeRecordsHandler>().AsSingle().NonLazy();
    }
}
