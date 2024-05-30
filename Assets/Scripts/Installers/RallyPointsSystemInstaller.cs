using Zenject;

public class RallyPointsSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        RallyPointSystem rallyPointSystem = new RallyPointSystem();
        Container.Bind<IRallyPointsHolder>().To<RallyPointSystem>().FromInstance(rallyPointSystem);
        Container.Bind<IRallyPointsChain>().To<RallyPointSystem>().FromInstance(rallyPointSystem);
        Container.Bind<IRallyPointsGizmosDrawer>().To<RallyPointSystem>().FromInstance(rallyPointSystem);
    }
}