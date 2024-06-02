using CustomUI;
using System;
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
    [Inject] private GameSettings _gameSettings;

    protected override void RegisterEvents(IUIEventsRegistrator registrator)
    {
        _musicVolumeSlider.value = _volumeController.MusicVolume;
        _effectsVolumeSlider.value = _volumeController.SFXVolume;
        _invertUpDownToggle.isOn = _gameSettings.InvertUpDown;

        registrator.RegisterButtonClickEvent(_okButton, OnOkButtonClicked);
        registrator.RegisterSliderChangeEvent(_musicVolumeSlider, OnMusicVolumeSliderChanged);
        registrator.RegisterSliderChangeEvent(_effectsVolumeSlider, OnEffectsVolumeSliderChanged);
        registrator.RegisterToggleChangeEvent(_invertUpDownToggle, OnInvertUpDownToggleChanged);
    }

    private void OnInvertUpDownToggleChanged(bool value)
    {
        _gameSettings.InvertUpDown = value;
    }

    private void OnEffectsVolumeSliderChanged(float value)
    {
        _volumeController.SFXVolume = _effectsVolumeSlider.value;
    }

    private void OnMusicVolumeSliderChanged(float value)
    {
        _volumeController.MusicVolume = _musicVolumeSlider.value;
    }

    private void OnOkButtonClicked()
    {
        CloseRequest();
    }
}