using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevelIsAvaliable(){

    }

    public void PrepareLevel()
    {
        Debug.Log("PrepareLevel()");
    }

    public void UnloadLevel()
    {
        Debug.Log("ActivateLevel()");
    }
}
