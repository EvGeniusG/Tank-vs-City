using UnityEngine;
using UniRx;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }

    // Реактивные переменные для джойстиков
    public ReactiveProperty<Vector2> MoveInput { get; private set; }
    public ReactiveProperty<Vector2> TargetInput { get; private set; }

    // Триггер для кнопки
    public Subject<Unit> ButtonTrigger { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Инициализация реактивных переменных
            MoveInput = new ReactiveProperty<Vector2>(Vector2.zero);
            TargetInput = new ReactiveProperty<Vector2>(Vector2.zero);

            // Инициализация триггера
            ButtonTrigger = new Subject<Unit>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetMove(Vector2 Move)
    {
        MoveInput.Value = Move;
    }
    
    public void SetTarget(Vector2 Target)
    {
        TargetInput.Value = Target;
    }
    // Метод для обнуления реактивных переменных
    public void ResetInputs()
    {
        MoveInput.Value = Vector2.zero;
        TargetInput.Value = Vector2.zero;
    }

    // Метод для активации триггера
    public void ActivateButtonTrigger()
    {
        ButtonTrigger.OnNext(Unit.Default);
    }
}
