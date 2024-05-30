using UnityEngine;
using Zenject;
using Cinemachine;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private AirPlane _plane;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public override void InstallBindings()
    {
        Container.BindInstance(_plane);
    }

    private void Reset()
    {
        if (_virtualCamera == null)
        {
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            BindVirtualCameraToPlane();
        }
    }

    private void OnValidate()
    {
        BindVirtualCameraToPlane();
    }

    private void BindVirtualCameraToPlane()
    {
        if (_virtualCamera != null) 
        { 
            _virtualCamera.Follow ??= _plane.transform;
            _virtualCamera.LookAt ??= _plane.transform;
        }
    }
}