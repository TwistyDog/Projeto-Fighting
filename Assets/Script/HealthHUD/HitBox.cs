using UnityEngine;

public class HitBox : MonoBehaviour
{

    private int _damage;
    private CombatSide.Side _targetSide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(int damage, CombatSide.Side targetSide)
    {
        _damage = damage;
        _targetSide = targetSide;

        Debug.Log(
            $"{gameObject.name} configurou HitBox | " +
            $"Dano: {_damage} | " +
            $"Alvo: {_targetSide}"
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver receiver =
            other.GetComponent<DamageReceiver>();

        if (receiver == null)
        {
            Debug.Log(
                $"HitBox {gameObject.name}: " +
                $"Collider {other.name} não possui DamageReceiver."
            );

            return;
        }

        CombatSide otherSide =
            receiver.GetComponent<CombatSide>();

        if (otherSide == null)
        {
            Debug.LogError(
                $"{receiver.gameObject.name} possui DamageReceiver " +
                $"mas não possui CombatSide!"
            );

            return;
        }

        Debug.Log(
            $"HitBox: {gameObject.name} atingiu " +
            $"{receiver.gameObject.name} | " +
            $"Lado alvo: {otherSide.CurrentSide} | " +
            $"Lado esperado: {_targetSide}"
        );

        if (otherSide.CurrentSide != _targetSide)
        {
            Debug.Log(
                "Ataque ignorado: lado incompatível."
            );

            return;
        }

        receiver.ReceiveDamaged(_damage);
    }
    
}
