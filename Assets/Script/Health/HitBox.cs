using UnityEngine;

public class HitBox : MonoBehaviour
{

    private int _damage;
    private string _targetTag = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(int damage, string targetTag)
    {
        _damage = damage;
        _targetTag = targetTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(_targetTag)) return;
        if (!other.CompareTag(_targetTag))return;

        if(other.TryGetComponent(out DamageReceiver receiver))
        {
            receiver.ReceiveDamaged(_damage);
        }
    }
}
