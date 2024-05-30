using UnityEngine;
using UnityEngine.Audio;

public class SoundVolumeController
{
    public float MusicVolume
    {
        get
        {
            _mixer.GetFloat(_musicVolumeParameterName, out float internalVolume);
            return GetExternalVolume(internalVolume);
        }
        set
        {
            _mixer.SetFloat(_musicVolumeParameterName, GetInternalVolume(value));
        }
    }

    public float SFXVolume
    {
        get
        {
            _mixer.GetFloat(_sfxVolumeParameterName, out float internalVolume);
            return GetExternalVolume(internalVolume);
        }
        set
        {
            _mixer.SetFloat(_sfxVolumeParameterName, GetInternalVolume(value));
        }
    }

    private AudioMixer _mixer;
    private string _musicVolumeParameterName;
    private string _sfxVolumeParameterName;

    public SoundVolumeController(AudioMixer mixer, string musicVolumeParameterName, string sfxVolumeParameterName)
    {
        _mixer = mixer;
        _musicVolumeParameterName = musicVolumeParameterName;
        _sfxVolumeParameterName = sfxVolumeParameterName;
    }

    private float GetInternalVolume(float externalVolume)
    {
        float tempValue = Mathf.Lerp(0.0001f, 1f, externalVolume);
        return Mathf.Log10(tempValue) * 20;  // by formula of dB to dinamic range: 20*lg(U1/U2)
    }

    private float GetExternalVolume(float internalVolume)
    {
        float tempValue = Mathf.Pow(10, (internalVolume / 20));
        return Mathf.InverseLerp(0.0001f, 1f, tempValue);
    }
}