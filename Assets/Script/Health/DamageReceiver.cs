using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private bool _isBlocking;
    [SerializeField, Range(0f,1f)] private float _blockReduction = 1f;

    private HealthForAll _health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _health = GetComponent<HealthForAll>();
    }

    public void SetBlocking(bool block)
    {
        _isBlocking = block;
    }

    public void Morrer()
    {
        bool isPlayer = CompareTag("Player");
        UITextFight.instance.OnKO(isPlayer);
    }

    public void ReceiveDamaged(int damage)
    {
        if (_health == null) return;

        if (_isBlocking)
        {
            damage = Mathf.RoundToInt(damage *(1f - _blockReduction));
            Debug.Log($"{gameObject.name} bloqueou o ataque");
        }

        if(damage > 0)
        _health.TakeDamage(damage);

        Debug.Log("Recebeu dano. Blocking: " + _isBlocking);
    }
}
