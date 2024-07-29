using UnityEngine;
using UniRx;
using UnityEngine.AI;

[RequireComponent(typeof(TankModel)), RequireComponent(typeof(TankView)), RequireComponent(typeof(NavMeshAgent))]
public class TankController : MonoBehaviour
{
    private TankModel model;

    NavMeshAgent nma;
    [SerializeField] private Transform turretTransform; // Добавляем ссылку на трансформ башни

    [SerializeField] private float rotationThreshold = 10f; // Точность до десятка градусов

    private void Awake()
    {
        model = GetComponent<TankModel>();
        nma = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Привязка камеры к танку
        CameraController.Instance.setTank(transform);
        
        // Подписка на обновление фиксированных кадров
        Observable.EveryFixedUpdate()
            .Subscribe(_ => {
                if (GamePlayController.Instance.MoveInput.Value == Vector2.zero)
                {
                    StopTank();
                }
                else
                {
                    OnMoveInputChanged(GamePlayController.Instance.MoveInput.Value);
                }

                if (GamePlayController.Instance.TargetInput.Value != Vector2.zero)
                {
                    OnTargetInputChanged(GamePlayController.Instance.TargetInput.Value);
                }
            })
            .AddTo(this);
    }

    private void OnMoveInputChanged(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            // Определение направления относительно камеры
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = forward * moveInput.y + right * moveInput.x;
            desiredMoveDirection.Normalize();

            // Движение танка
            MoveTank(desiredMoveDirection);

            // Поворот танка
            RotateTank(desiredMoveDirection);
        }
    }

    private void MoveTank(Vector3 moveDirection)
    {
        // Движение танка
        nma.isStopped = false;
        nma.SetDestination(transform.position + moveDirection * model.Speed * Time.deltaTime);
    }

    private void RotateTank(Vector3 moveDirection)
    {
        // Определение направления и угла поворота
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Поворот танка
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        if (angle > rotationThreshold)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * model.Maneuverability);
        }
        else
        {
            transform.rotation = targetRotation;
        }
    }

    private void StopTank()
    {
        // Остановка движения танка
        nma.isStopped = true;
    }

    private void OnTargetInputChanged(Vector2 targetInput)
    {
        if (targetInput != Vector2.zero)
        {
            RotateTurret(targetInput);
        }
    }

    private void RotateTurret(Vector2 targetInput)
    {
        // Определение направления относительно камеры
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredTargetDirection = forward * targetInput.y + right * targetInput.x;
        desiredTargetDirection.Normalize();

        // Определение направления и угла поворота башни
        Quaternion targetRotation = Quaternion.LookRotation(desiredTargetDirection);

        // Поворот башни
        float angle = Quaternion.Angle(turretTransform.rotation, targetRotation);
        if (angle > rotationThreshold)
        {
            turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, targetRotation, Time.deltaTime * model.Maneuverability);
        }
        else
        {
            turretTransform.rotation = targetRotation;
        }
    }

}
