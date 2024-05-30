using CustomUI;
using UnityEngine;
using Zenject;

public class MainMenuUIHandler : UIHandler
{
    [SerializeField] private MainMenuPanel _mainMenuPanelPrefab;
    [SerializeField] private Transform _parentPanel;

    [Inject] private SoundEffectsSource _soundEffectsSource;

    protected override Transform GetParentPanel() => _parentPanel;

    protected override void OnClick()
    {
        _soundEffectsSource.PlayClick();
    }

    protected override void OnToggleChanged(bool value)
    {
        OnClick();
    }

    protected override void OnEnterScene()
    {

    }

    protected override void OnExitScene()
    {
    }

    private void Awake()
    {
        OpenPanel(_mainMenuPanelPrefab);
    }
}
