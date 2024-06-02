using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CustomUI
{
    public abstract class UIHandler : MonoBehaviour
    {
        [Inject] private IInstantiator _instantiator;

        private Action _onPanelsSessionFinishCallback;
        private IPanel _activePanel;
        private Stack<IPanel> _openedMenuPanelPrefabs = new Stack<IPanel>();

        protected abstract Transform GetParentPanel();
        protected abstract void OnEnterScene();
        protected abstract void OnExitScene();
        protected abstract void OnClick();
        protected abstract void OnToggleChanged(bool value);

        protected void OpenPanel(IPanel panelPrefab, Action onPanelsSessionFinish)
        {
            _onPanelsSessionFinishCallback = onPanelsSessionFinish;
            OpenPanel(panelPrefab);
        }

        public void EnterScene()
        {
            OnEnterScene();
        }

        public virtual void ExitScene()
        {
            if (_activePanel != null)
            {
                UnbindPanelEvents(_activePanel);
            }
            OnExitScene();
        }

        protected void OpenPanel(IPanel panelPrefab)
        {
            if (_activePanel != null && _activePanel.Equals(panelPrefab))
            {
                throw new InvalidOperationException($"{panelPrefab} is already opened!");
            }

            if (_activePanel != null)
            {
                DestroyActivePanel();
            }

            SafeAddOpenedPanelPrefab(panelPrefab);
            IPanel newPanel = _instantiator.InstantiatePrefabForComponent<IPanel>(panelPrefab.GetGameObject(), GetParentPanel());
            BindPanelEvents(newPanel);
            _activePanel = newPanel;
        }

        private void CloseActivePanel()
        {
            _openedMenuPanelPrefabs.Pop();
            if (_openedMenuPanelPrefabs.Count == 0)
            {
                DestroyActivePanel();
                _onPanelsSessionFinishCallback?.Invoke();
            }
            else
            {
                OpenPanel(_openedMenuPanelPrefabs.Peek());
            }
        }

        private void DestroyActivePanel()
        {
            UnbindPanelEvents(_activePanel);
            Destroy(_activePanel.GetGameObject());
            _activePanel = null;
        }

        private void SafeAddOpenedPanelPrefab(IPanel panel)
        {
            if (_openedMenuPanelPrefabs.Contains(panel))
            {
                while (_openedMenuPanelPrefabs.Peek() != panel)
                {
                    _openedMenuPanelPrefabs.Pop();
                }
            }
            else
            {
                _openedMenuPanelPrefabs.Push(panel);
            }
        }

        private void BindPanelEvents(IPanel panel)
        {
            panel.OnClick += OnClick;
            panel.OnToggleChanged += OnToggleChanged;
            panel.OnCloseRequest += CloseActivePanel;
            panel.OnOpenPanelRequest += OpenPanel;
        }

        private void UnbindPanelEvents(IPanel panel)
        {
            panel.OnClick -= OnClick;
            panel.OnToggleChanged -= OnToggleChanged;
            panel.OnCloseRequest -= CloseActivePanel;
            panel.OnOpenPanelRequest -= OpenPanel;
        }
    }
}