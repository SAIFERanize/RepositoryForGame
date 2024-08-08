using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public float damage = 1.0f; // Количество урона

    void OnCollisionEnter2D(Collision2D collision)
    {
        player playerHealth = collision.collider.GetComponent<player>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        player playerHealth = other.GetComponent<player>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
