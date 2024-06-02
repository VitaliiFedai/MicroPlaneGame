using System.Threading.Tasks;
using Unity.VisualScripting;
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

    public void PlayWinSound()
    {
        _sfxSource.PlayOneShot(_winSound);
    }

    public void PlayLoseSound()
    {
        _sfxSource.PlayOneShot(_loseSound);
    }
}
