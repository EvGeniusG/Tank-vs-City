public class MainMenuState : State
{
    public override void Enter()
    {
        UIManager.Instance.ShowMainMenu();
        LevelManager.Instance.PrepareLevel();
    }

    public override void Exit()
    {
        UIManager.Instance.HideMainMenu();
    }

    public override void Update()
    {
        // Handle main menu updates
    }
}