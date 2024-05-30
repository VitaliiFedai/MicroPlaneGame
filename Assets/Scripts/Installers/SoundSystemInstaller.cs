using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundSystemInstaller : MonoInstaller
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _musicVolumeParameterName;
    [SerializeField] private string _sfxVolumeParameterName;
    [SerializeField] private SoundEffectsSource _soundEffectSource;

    public override void InstallBindings()
    {
        Container.Bind<IInstantiator>().To<DiContainer>().FromInstance(Container).AsSingle();

        Container.BindInstance(_audioMixer).AsSingle();
        Container.Bind<SoundVolumeController>().AsSingle().WithArguments(_audioMixer, _musicVolumeParameterName, _sfxVolumeParameterName).NonLazy();
        Container.BindInstance(_soundEffectSource).AsSingle();
    }
}