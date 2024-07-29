using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float health = 100f; // Начальное здоровье объекта
    public GameObject destroyedVersion; // Объект, который будет появляться после уничтожения

    public void TakeDamage(IDamageSource damageSource)
    {
        float damageAmount = damageSource.GetDamageAmount();
        health -= damageAmount;

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        // Логика уничтожения объекта
        if (destroyedVersion != null)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
