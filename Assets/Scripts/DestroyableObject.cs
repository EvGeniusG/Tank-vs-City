using UnityEngine;

public abstract class DestroyableObject : MonoBehaviour, IDestroyable
{
    public float health = 100f;
    public GameObject destroyedVersion;

    public void TakeDamage(IDamageSource damageSource)
    {
        float damageAmount = damageSource.GetDamageAmount();
        health -= damageAmount;

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    protected virtual void DestroyObject()
    {
        if (destroyedVersion != null)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
