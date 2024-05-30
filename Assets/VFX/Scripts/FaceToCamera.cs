using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        RefreshRotation();
    }

    private void RefreshRotation()
    {
        transform.up = Camera.main.transform.forward;
    }
}
