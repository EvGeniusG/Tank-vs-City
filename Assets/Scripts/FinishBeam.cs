using UnityEngine;

public class FinishBeam : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.attachedRigidbody == null) return;
        
        if(other.attachedRigidbody.CompareTag("Player")){
            GameManager.Instance.ChangeState(new WinState());
        }
    }
}
