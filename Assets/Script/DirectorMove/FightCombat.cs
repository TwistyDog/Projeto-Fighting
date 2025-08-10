using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightCombat : MonoBehaviour
{
    private bool _isAtacking = false;
    private float _attackCooldown = 0.3f;

    // golpes
    [Header("HitBoxes")]
    [SerializeField] private GameObject _rightPunchHitbox;
    [SerializeField] private GameObject _leftPunchHitbox;
    [SerializeField] private GameObject _hightKickHitbox;
    [SerializeField] private GameObject _lowKickHitbox;

    // Dano dos golpes
     [Header("Damage")]
    [SerializeField] private int _rightPunchDamage = 10;
    [SerializeField] private int _leftPunchDamage = 12;
    [SerializeField] private int _highKickDamage = 15;
    [SerializeField] private int _lowKickDamage = 8;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Combat");
    }

    public void OnRightPunch(InputAction.CallbackContext context)
    {
        if (context.performed && !_isAtacking)
        {
            Debug.Log("Soco Direito");
            RightPuch();
        }
    }
    public void OnLeftPunch(InputAction.CallbackContext context)
    {
        if (context.performed && !_isAtacking)
        {
            Debug.Log("Soco Esquerdo");
            LeftPuch();
        }
    }
    public void OnHightKick(InputAction.CallbackContext context)
     {
        HighKick();
     }
    public void OnLowKick(InputAction.CallbackContext context)
     {
        LowKick();
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

    void ResetAttack() => _isAtacking = false;
    
}
