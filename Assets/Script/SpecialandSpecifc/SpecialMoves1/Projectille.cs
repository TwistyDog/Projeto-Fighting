using UnityEngine;

public class Projectille : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Vector3 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OiggerEnter(Collider other)
    {
       var _damageReceiver = other.GetComponent<DamageReceiver>();

       if(_damageReceiver != null)
        {
            _damageReceiver.ReceiveDamaged(damage);

        } 

        Destroy(gameObject);
    }
}
