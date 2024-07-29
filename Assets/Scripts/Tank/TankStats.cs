using UnityEngine;

[CreateAssetMenu(fileName = "NewTankStats", menuName = "Tank/TankStats")]
public class TankStats : ScriptableObject
{
    [Header("Health")]
    public int health;

    [Header("Damage")]
    public int damage;

    [Header("Attack Speed")]
    public float attackSpeed;

    [Header("Power")]
    public int power;

    [Header("Speed")]
    public float speed;

    [Header("Ramming Force")]
    public int rammingForce;

    [Header("Maneuverability")]
    public float maneuverability;
}
