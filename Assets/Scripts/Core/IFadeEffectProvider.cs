using System.Threading.Tasks;
using UnityEngine;

public interface IFadeEffectProvider
{
    public Task FadeOutEffect(Transform parent);
    public Task FadeOutEffect(Transform parent, float duration);
    public Task<GameObject> FadeInEffect(Transform parent);
    public Task<GameObject> FadeInEffect(Transform parent, float duration);
}
