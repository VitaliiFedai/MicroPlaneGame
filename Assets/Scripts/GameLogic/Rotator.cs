using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateSpeed;

    private void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime);
    }
}
