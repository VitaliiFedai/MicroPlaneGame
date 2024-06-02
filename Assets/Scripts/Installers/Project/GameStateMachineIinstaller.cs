using CustomUI;
using UnityEngine;
using Zenject;

public class GameStateMachineIinstaller : MonoInstaller
{
    [SerializeField] private float _fadeEffectDuration = 3f;
    [SerializeField] private FadeInOutPanel _fadePrefab;
    [SerializeField] private GameObject _explosionPrefab;

    public override void InstallBindings()
    {
        FadeEffect fadeEffect = new FadeEffect(_fadePrefab, _fadeEffectDuration);

        Container.Bind<GameplayState>().AsSingle().WithArguments(fadeEffect, _explosionPrefab, "Gameplay").NonLazy();
        Container.Bind<MainMenuState>().AsSingle().WithArguments(fadeEffect).NonLazy();
        Container.Bind<PlaneGameStateMachine>().AsSingle();
    }
}