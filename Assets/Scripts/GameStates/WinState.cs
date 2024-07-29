using UnityEngine;

public class WinState : State
{
    public override void Enter()
    {
        UIManager.Instance.ShowWinScreen();
        GamePlayController.Instance.SetMove(Vector2.zero);
        GamePlayController.Instance.SetTarget(Vector2.zero);
        LevelManager.Instance.NextLevelIsAvaliable();
    }

    public override void Exit()
    {
        UIManager.Instance.HideWinScreen();
        LevelManager.Instance.UnloadLevel();
    }

    public override void Update()
    {
        // Handle win state updates
    }
}