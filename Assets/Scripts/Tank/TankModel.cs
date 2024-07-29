using UnityEngine;
using UniRx;

public class TankModel : MonoBehaviour
{
    [SerializeField] private TankStats tankStats;

    public int Health => tankStats.health;
    public int Damage => tankStats.damage;
    public float AttackSpeed => tankStats.attackSpeed;
    public int Power => tankStats.power;
    public float Speed => tankStats.speed;
    public int RammingForce => tankStats.rammingForce;
    public float Maneuverability => tankStats.maneuverability;

    public ReactiveProperty<int> CurrentHealth { get; private set; }
    public ReactiveProperty<bool> IsAlive { get; private set; }

    private void Awake()
    {
        // Initialize reactive properties
        CurrentHealth = new ReactiveProperty<int>(Health);
        IsAlive = new ReactiveProperty<bool>(true);

        // Subscribe to health changes to update IsAlive status
        CurrentHealth
            .Where(h => h <= 0)
            .Subscribe(_ => IsAlive.Value = false)
            .AddTo(this);
    }

    public void TakeDamage(int amount)
    {
        if (IsAlive.Value) // Only take damage if the tank is alive
        {
            CurrentHealth.Value -= amount;
        }
    }
}
