using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int _health = 100;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"Inimigo tomou {damage} de vida! Vida restante: {_health}");

        if (_health <= 0)
        {
            Destroy(gameObject);
        }

    }
}

