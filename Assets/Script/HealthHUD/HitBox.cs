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
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver receiver =
            other.GetComponent<DamageReceiver>();

        if (receiver == null)
            return;


        CombatSide otherSide =
            other.GetComponent<CombatSide>();


        if (otherSide == null)
            return;


        if (otherSide.CurrentSide != _targetSide)
            return;


        receiver.ReceiveDamaged(_damage);
    }
}
