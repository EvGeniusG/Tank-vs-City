using UnityEngine;

public class Projectile : MonoBehaviour, IDamageSource
{
    public float damageAmount = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        IDestroyable destroyable = collision.gameObject.GetComponent<IDestroyable>();
        if (destroyable != null)
        {
            destroyable.TakeDamage(this);
        }

        // Уничтожить снаряд после столкновения
        Destroy(gameObject);
    }

    public float GetDamageAmount()
    {
        return damageAmount;
    }
}
