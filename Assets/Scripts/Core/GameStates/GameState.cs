using CustomUI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameState : IGameState
{
    public event Action OnEnter;
    public event Action OnExit;

    private const string UI_CANVAS_TAG = "UICanvas";
    private UIHandler _panelHandler;
    private Canvas _canvas;
    private FadeEffect _fadeEffect;
    private Scene _scene;

    public GameState(FadeEffect fadeEffect)
    {
        _fadeEffect = fadeEffect;
    }

    public async Task Enter()
    {
        await LoadScene();
        _panelHandler = UnityEngine.Object.FindObjectOfType<UIHandler>();
        OnEnter += _panelHandler.EnterScene;
        OnExit += _panelHandler.ExitScene;

        OnLoadSceneFinished();

        if (!FindUICanvas(out _canvas))
        {
            Debug.LogError($"Can''t find {nameof(Canvas)} with tag \"{UI_CANVAS_TAG}\"!");
        }

        await _fadeEffect.FadeOutEffect(_canvas.transform);
        OnEnterPerformed();
    }

    public async Task Exit()
    {
        OnExitPerformed();
        OnEnter -= _panelHandler.EnterScene;
        OnExit -= _panelHandler.ExitScene;
        await _fadeEffect.FadeInEffect(_canvas.transform);
        _canvas = null;
        _panelHandler = null;
    }

    public void Update()
    {
        OnUpdate();
    }

    protected abstract string GetSceneName();

    protected virtual void OnLoadSceneFinished()
    {
    }

    protected virtual void OnEnterPerformed()
    {
        OnEnter?.Invoke();
    }

    protected virtual void OnExitPerformed()
    {
        OnExit?.Invoke();
    }

    protected virtual void OnUpdate() 
    { 
    }

    private async Task LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(GetSceneName(), LoadSceneMode.Single);
        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }

    private bool FindUICanvas(out Canvas canvas)
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag(UI_CANVAS_TAG);
        if (gameObject != null && gameObject.TryGetComponent(out Canvas canvasComponent))
        {
            canvas = canvasComponent;
            return true;
        }
        else
        {
            canvas = null;
            return false;
        }
    }
}