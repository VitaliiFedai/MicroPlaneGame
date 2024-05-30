using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomUI
{
    public class UIEventsBinder : IUIEventsRegistrator 
    {
        private Dictionary<Button, UnityAction> _onClickEvents = new Dictionary<Button, UnityAction>();
        private Dictionary<Toggle, UnityAction<bool>> _onToggleChangeEvents = new Dictionary<Toggle, UnityAction<bool>>();
        private Dictionary<Slider, UnityAction<float>> _onSliderChangeEvents = new Dictionary<Slider, UnityAction<float>>();

        private UnityAction _sharedOnButtonClick;
        private UnityAction<bool> _sharedOnToggleChanged;

        public UIEventsBinder(UnityAction sharedOnButtonClick, UnityAction<bool> sharedOnToggleChanged)
        {
            _sharedOnButtonClick = sharedOnButtonClick;
            _sharedOnToggleChanged = sharedOnToggleChanged;
        }

        public void RegisterButtonClickEvent(Button button, UnityAction callback)
        {
            if (!_onClickEvents.ContainsKey(button))
            {
                _onClickEvents[button] = null;
            }
            _onClickEvents[button] += callback;
        }

        public void RegisterToggleChangeEvent(Toggle toggle, UnityAction<bool> callback)
        {
            if (!_onToggleChangeEvents.ContainsKey(toggle))
            {
                _onToggleChangeEvents[toggle] = null;
            }
            _onToggleChangeEvents[toggle] += callback;
        }

        public void RegisterSliderChangeEvent(Slider slider, UnityAction<float> callback)
        {
            if (!_onSliderChangeEvents.ContainsKey(slider))
            {
                _onSliderChangeEvents[slider] = null;
            }
            _onSliderChangeEvents[slider] += callback;
        }

        public void BindListeners()
        {
            BindButtons();
            BindToggles();
            BindSliders();
        }

        public void UnbindListeners()
        {
            UnbindButtons();
            UnbindToggles();
            UnbindSliders();
        }

        private void BindButtons()
        {
            foreach (Button button in _onClickEvents.Keys)
            {
                if (_sharedOnButtonClick != null)
                {
                    button.onClick.AddListener(_sharedOnButtonClick);
                }
                button.onClick.AddListener(_onClickEvents[button]);
            }
        }

        private void BindToggles()
        {
            foreach (Toggle toggle in _onToggleChangeEvents.Keys)
            {
                if (_sharedOnToggleChanged != null)
                { 
                    toggle.onValueChanged.AddListener(_sharedOnToggleChanged);
                }
                toggle.onValueChanged.AddListener(_onToggleChangeEvents[toggle]);
            }
        }

        private void BindSliders()
        {
            foreach (Slider slider in _onSliderChangeEvents.Keys)
            {
                slider.onValueChanged.AddListener(_onSliderChangeEvents[slider]);
            }
        }

        private void UnbindButtons()
        {
            foreach (Button button in _onClickEvents.Keys)
            {
                button.onClick.RemoveListener(_sharedOnButtonClick);
                button.onClick.RemoveListener(_onClickEvents[button]);
            }
        }

        private void UnbindToggles()
        {
            foreach (Toggle toggle in _onToggleChangeEvents.Keys)
            {
                toggle.onValueChanged.RemoveListener(_sharedOnToggleChanged);
                toggle.onValueChanged.RemoveListener(_onToggleChangeEvents[toggle]);
            }
        }

        private void UnbindSliders()
        {
            foreach (Slider slider in _onSliderChangeEvents.Keys)
            {
                slider.onValueChanged.AddListener(_onSliderChangeEvents[slider]);
            }
        }
    }
}