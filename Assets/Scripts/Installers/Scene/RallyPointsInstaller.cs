using UnityEngine;
using Zenject;

public class RallyPointsInstaller : MonoInstaller
{
    [SerializeField] private RallyPoint[] _rallyPoints;
    [Inject] private IRallyPointsHolder _rallyPointsHolder;
    [Inject] private IRallyPointsGizmosDrawer _rallyPointsGizmosDrawer;
    
    public override void InstallBindings()
    {
    }

    private void OnEnable()
    {
        _rallyPointsHolder.SetRallyPoints(_rallyPoints);
    }

    private void OnDisable()
    {
        _rallyPointsHolder.ClearRallyPoints();
    }

    private void OnDrawGizmos()
    {
        if (_rallyPointsGizmosDrawer != null)
        { 
            _rallyPointsGizmosDrawer.DrawGizmos(); 
        }
    }
}