using System;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(AirPlane))]
public class PlaneEngineSoundController : MonoBehaviour
{
    private const float DEFAULT_PITCH = 1f;
    
    [Range(0f, 1f)]
    [SerializeField] private float _minSpeedPitch;
    [Range(1f, 3f)]
    [SerializeField] private float _maxSpeedPitch;

    private AudioSource _audioSource;
    private AirPlane _airPlane;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _airPlane = GetComponent<AirPlane>();
    }

    private void Update()
    {
        _audioSource.pitch = GetPitch();
    }

    private float GetPitch()
    {
        bool isFasterThanDefault = _airPlane.Speed > _airPlane.DefaultSpeed;
        return isFasterThanDefault ? GetPitchTowardsSpeed(_airPlane.MaxSpeed, _maxSpeedPitch) : GetPitchTowardsSpeed(_airPlane.MinSpeed, _minSpeedPitch);
    }

    private float GetPitchTowardsSpeed(float targetSpeed, float targetSpeedPitch)
    {
        float weight = Mathf.InverseLerp(_airPlane.DefaultSpeed, targetSpeed, _airPlane.Speed);
        return Mathf.Lerp(DEFAULT_PITCH, targetSpeedPitch, weight);
    }
}