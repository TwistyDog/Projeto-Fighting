using UnityEditor;
using UnityEngine;

public class FightCombat : MonoBehaviour
{
    private bool _isAtacking = false;
    private float _attackCooldown = 0.3f;

    // golpes
    [SerializeField] private GameObject _rightPunchHitbox;
    [SerializeField] private GameObject _leftPunchHitbox;
    [SerializeField] private GameObject _hightKickHitbox;
    [SerializeField] private GameObject _lowKickHitbox;

    // Dano dos golpes

    [SerializeField] private int _rightPunchDamage = 10;
    [SerializeField] private int _leftPunchDamage = 12;
    [SerializeField] private int _highKickDamage = 15;
    [SerializeField] private int _lowKickDamage = 8;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnDrawGizmos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAtacking)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                RightPuch();
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            LeftPuch();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            HighKick();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            LowKick();
        }

        
    }

    void RightPuch()
    {
        _isAtacking = true;
        Debug.Log("Soco Direito");
        ActiveHitBox(_rightPunchHitbox, _rightPunchDamage);
        Invoke("ResetAttack", _attackCooldown);
    }

    void LeftPuch()
    {
        _isAtacking = true;
        Debug.Log("Soco Esquerdo");
        ActiveHitBox(_leftPunchHitbox, _leftPunchDamage);
        Invoke("ResetAttack", _attackCooldown);
    }
    void HighKick()
    {
        _isAtacking = true;
        Debug.Log("Chute Alto");
        ActiveHitBox(_hightKickHitbox, _highKickDamage);
        Invoke("ResetAttack", _attackCooldown);
    }

    void LowKick()
    {
        _isAtacking = true;
        Debug.Log("Chute baixo");
        ActiveHitBox(_lowKickHitbox, _lowKickDamage);
        Invoke("ResetAttack", _attackCooldown);
    }

    void ActiveHitBox(GameObject hitbox, int damage)
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true);
            Collider[] hitEnemies = Physics.OverlapBox(hitbox.transform.position, hitbox.transform.localScale / 2, hitbox.transform.rotation);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
        }
    }
    void DrawHitBoxGizmo(GameObject hitbox, Color color)
    {
        if (hitbox != null && hitbox.activeSelf)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(hitbox.transform.position, hitbox.transform.localScale);
        }
    }

    private void OnDrawGizmos()
    {
        DrawHitBoxGizmo(_rightPunchHitbox, Color.red);
        DrawHitBoxGizmo(_leftPunchHitbox, Color.blue);
        DrawHitBoxGizmo(_lowKickHitbox, Color.green);
        DrawHitBoxGizmo(_hightKickHitbox, Color.yellow);
    }
    void ResetAttack()
    {
        _isAtacking = false;
    }
}
