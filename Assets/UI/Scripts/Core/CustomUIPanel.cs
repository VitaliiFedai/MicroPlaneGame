using HelpUtils;
using System;
using UnityEngine;

namespace CustomUI
{
    public abstract class CustomUIPanel : MonoBehaviour, IPanel
    {
        public event Action OnClick;
        public event Action<bool> OnToggleChanged;

        public event Action OnCloseRequest;
        public event Action<IPanel> OnOpenPanelRequest;

        private UIEventsBinder _eventsBinder;

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        protected abstract void RegisterEvents(IUIEventsRegistrator registrator);

        protected void OpenPanelRequest(IPanel panelPrefab)
        {
            #if UNITY_EDITOR
            if (!Validator.IsPrefab(panelPrefab.GetGameObject()))
            {
                throw new ArgumentException($"{nameof(panelPrefab)} == {panelPrefab}. It is not a part of Prefab!");
            }
            #endif
            OnOpenPanelRequest?.Invoke(panelPrefab);
        }

        protected void CloseRequest()
        {
            OnCloseRequest?.Invoke();
        }

        private void Awake()
        {
            _eventsBinder = new UIEventsBinder(OnButtonClickPreformed, OnToggleChangedPerformed);
            RegisterEvents(_eventsBinder);
        }

        private void OnEnable()
        {
            _eventsBinder.BindListeners();
        }

        private void OnDisable()
        {
            _eventsBinder.UnbindListeners();
        }

        private void OnButtonClickPreformed()
        {
            OnClick?.Invoke();
        }

        private void OnToggleChangedPerformed(bool value)
        {
            OnToggleChanged?.Invoke(value);
        }
    }
}