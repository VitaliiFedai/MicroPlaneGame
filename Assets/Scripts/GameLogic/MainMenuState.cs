public class MainMenuState : GameState
{
    private const string SCENE_NAME = "MainMenu";

    public MainMenuState(FadeEffect fadeEffect) : base(fadeEffect)
    {
    }

    protected override string GetSceneName()
    {
        return SCENE_NAME;
    }
}
