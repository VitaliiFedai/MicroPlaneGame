using CustomUI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameState : IGameState
{
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
        Debug.Log($"{GetType().Name}.{nameof(Enter)}");

        await LoadScene();
        _panelHandler = Object.FindObjectOfType<UIHandler>();
        _canvas = Object.FindObjectOfType<Canvas>();

        await _fadeEffect.FadeOutEffect(_canvas.transform);
        _panelHandler?.EnterScene();
        OnEnter();
    }

    public async Task Exit()
    {
        OnExit();
        _panelHandler?.ExitScene();
        await _fadeEffect.FadeInEffect(_canvas.transform);
    }

    public void Update()
    {
        OnUpdate();
    }

    protected abstract string GetSceneName();

    protected virtual void OnEnter()
    {
    }

    protected virtual void OnExit()
    {
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
}