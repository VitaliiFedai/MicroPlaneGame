using CustomUI;
using System;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using HelpUtils;
#endif

public class FadeEffect : IFadeEffectProvider
{
    private FadeInOutPanel _fadeinOutPanelPrefab;
    private float _defaultDuration;

    public FadeEffect(FadeInOutPanel fadeinOutPanelPrefab, float defaultDuration)
    {
        #if UNITY_EDITOR
        if (!Validator.IsPrefab(fadeinOutPanelPrefab.gameObject))
        {
            throw new ArgumentException(nameof(fadeinOutPanelPrefab));
        }
        #endif
        _fadeinOutPanelPrefab = fadeinOutPanelPrefab;
        _defaultDuration = defaultDuration;
    }

    public async Task FadeOutEffect(Transform parent, float duration)
    {
        FadeInOutPanel fadeInOutPanel = UnityEngine.Object.Instantiate(_fadeinOutPanelPrefab, parent);
        await fadeInOutPanel.FadeOut(duration);
        UnityEngine.Object.Destroy(fadeInOutPanel.gameObject);
    }

    public async Task FadeOutEffect(Transform parent)
    {
        await FadeOutEffect(parent, _defaultDuration);
    }

    public async Task<GameObject> FadeInEffect(Transform parent, float duration)
    {
        FadeInOutPanel fadeInOutPanel = UnityEngine.Object.Instantiate(_fadeinOutPanelPrefab, parent);
        await fadeInOutPanel.FadeIn(duration);
        return fadeInOutPanel.gameObject;
    }

    public async Task<GameObject> FadeInEffect(Transform parent)
    {
        return await FadeInEffect(parent, _defaultDuration);
    }
}
