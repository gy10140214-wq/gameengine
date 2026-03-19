using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} damaged! Remaining HP: {health}");

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 죽는 연출 또는 이펙트 가능
        Destroy(gameObject);
    }
}
