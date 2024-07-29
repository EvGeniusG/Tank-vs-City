public class LoseState : State
{
    public override void Enter()
    {
        UIManager.Instance.ShowLoseScreen();
    }

    public override void Exit()
    {
        UIManager.Instance.HideLoseScreen();
        LevelManager.Instance.UnloadLevel();
    }

    public override void Update()
    {
        // Handle lose state updates
    }
}