using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject winScreenUI;
    [SerializeField] private GameObject loseScreenUI;

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

    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        winScreenUI.SetActive(false);
        loseScreenUI.SetActive(false);
    }

    public void HideMainMenu()
    {
        mainMenuUI.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        gameplayUI.SetActive(true);
    }

    public void HideGameplayUI()
    {
        gameplayUI.SetActive(false);
    }

    public void ShowWinScreen()
    {
        winScreenUI.SetActive(true);
    }

    public void HideWinScreen()
    {
        winScreenUI.SetActive(false);
    }

    public void ShowLoseScreen()
    {
        loseScreenUI.SetActive(true);
    }

    public void HideLoseScreen()
    {
        loseScreenUI.SetActive(false);
    }
}
