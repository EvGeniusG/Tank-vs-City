using UnityEngine;

class TankCreator : MonoBehaviour{
    public static TankCreator Instance { get; private set;}

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] GameObject Tank;

    public void CreateTank(Vector3 position){
        Instantiate(Tank, position, Quaternion.identity);
    }
}