using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomUI
{
    public interface IUIEventsRegistrator
    {
        public void RegisterButtonClickEvent(Button button, UnityAction callback);
        public void RegisterToggleChangeEvent(Toggle toggle, UnityAction<bool> callback);
        public void RegisterSliderChangeEvent(Slider slider, UnityAction<float> callback);
    }
}