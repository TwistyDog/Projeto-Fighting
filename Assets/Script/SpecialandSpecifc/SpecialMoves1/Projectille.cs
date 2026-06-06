using UnityEngine;

public class Projectille : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    [SerializeField] private float arenaLimit = 12f;

    private Vector3 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        
    }

    void Update()
    {
        if(!GameManager.Instance.podeControlar)
            return;
        
        
        transform.position += direction * speed * Time.deltaTime;

        if(Mathf.Abs(transform.position.x) > arenaLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       var _damageReceiver = other.GetComponent<DamageReceiver>();

       if(_damageReceiver != null)
        {
            _damageReceiver.ReceiveDamaged(damage);

        } 

        Destroy(gameObject);
    }
}
