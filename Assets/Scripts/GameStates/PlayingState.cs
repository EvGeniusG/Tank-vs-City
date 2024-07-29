public class PlayingState : State
{
    public override void Enter()
    {
        UIManager.Instance.ShowGameplayUI();
        LevelManager.Instance.PrepareLevel();
    }

    public override void Exit()
    {
        UIManager.Instance.HideGameplayUI();
        LevelManager.Instance.UnloadLevel();
    }

    public override void Update()
    {
        // Handle gameplay updates
    }
}