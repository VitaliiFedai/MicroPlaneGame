using System.Threading.Tasks;
using UnityEngine;

public class DoorOpener : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _door;
    [SerializeField] private TriggerZone _exitTrigger;
    [SerializeField] private Vector3 _targetRotation = new Vector3(0, 30f, 0);
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private AudioSource _audioSource;    

    private bool _isInteracted;
    private Vector3 _initialEulers;

    public async void Interact()
    {
        if (_isInteracted || _door == null)
        {
            return;
        }
        _isInteracted = true;
 
        SetExitTriggerEnabled(true);
        _audioSource?.Play();

        Vector3 currentRotation = Vector3.zero;
        while (Vector3.Distance(currentRotation, _targetRotation) >= 0.001f)
        {
            currentRotation = Vector3.MoveTowards(currentRotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            _door.transform.eulerAngles = _initialEulers + currentRotation;
            await Task.Yield();
        }
        _door.transform.eulerAngles = _initialEulers + _targetRotation;
    }

    private void Awake()
    {
        SetExitTriggerEnabled(false);
        _initialEulers = transform.eulerAngles;
    }

    private void SetExitTriggerEnabled(bool value)
    {
        if (_exitTrigger != null)
        {
            _exitTrigger.enabled = value;
        }
    }

    private void Reset()
    {
        _door = transform;
    }
}