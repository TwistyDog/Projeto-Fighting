using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private bool _isBlocking;
    [SerializeField, Range(0f, 1f)] private float _blockReduction = 1f;

    private HealthForAll _health;
    private CombatSide _combatSide;

    private void Awake()
    {
        _health = GetComponent<HealthForAll>();
        _combatSide = GetComponent<CombatSide>();
    }

    public void SetBlocking(bool block)
    {
        _isBlocking = block;
    }

    public void Morrer()
    {
        if (UITextFight.instance == null)
        {
            Debug.LogError(
                "DamageReceiver: UITextFight não encontrado!"
            );

            return;
        }

        // Envia o próprio GameObject que morreu.
        UITextFight.instance.OnKO(gameObject);
    }

    public void ReceiveDamaged(int damage)
    {
        if (_health == null)
        {
            Debug.LogError(
                $"{gameObject.name}: HealthForAll não encontrado!"
            );

            return;
        }

        if (_isBlocking)
        {
            damage = Mathf.RoundToInt(
                damage * (1f - _blockReduction)
            );

            Debug.Log(
                $"{gameObject.name} bloqueou o ataque."
            );
        }

        if (damage > 0)
        {
            Debug.Log(
                $"{gameObject.name} recebeu {damage} de dano."
            );

            _health.TakeDamage(damage);
        }
    }
}
