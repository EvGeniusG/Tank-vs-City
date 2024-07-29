using UnityEngine;

public class StartGameButton : MonoBehaviour{
    public void ActivateButton(){
        GameManager.Instance.ChangeState(new PlayingState());
    }
}