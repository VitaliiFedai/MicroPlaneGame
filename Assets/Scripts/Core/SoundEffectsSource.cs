using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SoundEffectsSource : MonoBehaviour 
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxSource;

    [Header ("UI Sounds")]
    [SerializeField] private AudioClip _clickSound;

    [Header ("Gameplay Sounds")]
    [SerializeField] private AudioClip _checkPointSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    public void PlayClick()
    {
        _sfxSource.PlayOneShot(_clickSound);
    }

    public void PlayCheckPointSound()
    {
        _sfxSource.PlayOneShot(_checkPointSound);
    }

    public async void PlayWinSound(float delay)
    {
        await PlayDelayed(_winSound, delay);
    }

    public async void PlayLoseSound(float delay)
    {
        await PlayDelayed(_loseSound, delay);
    }

    private async Task PlayDelayed(AudioClip sound, float delay)
    {
        await Task.Delay((int)(delay * 1000));
        _sfxSource.PlayOneShot(sound);
    }
}
