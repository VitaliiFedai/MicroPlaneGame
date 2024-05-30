using System;
using UnityEngine;

namespace CustomUI
{
    public interface IPanel
    {
        public event Action OnClick;
        public event Action<bool> OnToggleChanged;

        public event Action OnCloseRequest;
        public event Action<Action> OnSceneExitRequest;
        public event Action<IPanel> OnOpenPanelRequest;
        public GameObject GetGameObject();
    }
}