using Unity.AI.Navigation;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{

    public static NavigationManager Instance { get; private set;}

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(gameObject);
        }
    }
    [SerializeField] NavMeshSurface navMeshSurface;

    public void BuildNavMesh(){
        navMeshSurface.BuildNavMesh();
    }
}
