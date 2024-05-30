using CustomUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsMenuPanel : CustomUIPanel
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private Toggle _invertUpDownToggle;

    [Inject] private SoundVolumeController _volumeController;

    protected override void RegisterEvents(IUIEventsRegistrator registrator)
    {
        _musicVolumeSlider.value = _volumeController.MusicVolume;
        _effectsVolumeSlider.value = _volumeController.SFXVolume;

        registrator.RegisterButtonClickEvent(_okButton, OnOkButtonClicked);
        registrator.RegisterSliderChangeEvent(_musicVolumeSlider, OnMusicVolumeSliderChanged);
        registrator.RegisterSliderChangeEvent(_effectsVolumeSlider, OnEffectsVolumeSliderChanged);
    }

    private void OnEffectsVolumeSliderChanged(float arg0)
    {
        _volumeController.SFXVolume = _effectsVolumeSlider.value;
    }

    private void OnMusicVolumeSliderChanged(float arg0)
    {
        _volumeController.MusicVolume = _musicVolumeSlider.value;
    }

    private void OnOkButtonClicked()
    {
        CloseRequest();
    }
}