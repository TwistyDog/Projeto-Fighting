using UnityEngine;

public class HitBox : MonoBehaviour
{

    private int _damage;
    private string _targetTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(int damage, string targetTag)
    {
        _damage = damage;
        _targetTag = targetTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_targetTag)) return;

        DamageReceiver receiver = other.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            receiver.ReceiveDamaged(_damage);
        }
    }
}
