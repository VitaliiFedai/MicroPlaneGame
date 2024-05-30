using UnityEngine;
using Zenject;

public class GameplayUIHandlerInstaller : MonoInstaller
{
    [SerializeField] private GameplayUIHandler _gameplayUIHandler;

    public override void InstallBindings()
    {
        Container.BindInstance(_gameplayUIHandler);
    }
}