using System;
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

    public bool IsAtacking => _isAtacking;


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
        if (playerInput != null) // troca o mapa de input se for player
        {
            playerInput.SwitchCurrentActionMap("Combat");
        }
    }

    //

    public void RightPuch()
    {
        if (_isAtacking) return;
        _isAtacking = true;
        Debug.Log($"{gameObject.name} deu soco direito");
        ActiveHitBox(_rightPunchHitbox, _rightPunchDamage);
        Invoke("ResetAttack", _attackCooldown);
    }

    public void LeftPuch()
    {
        if (_isAtacking) return;
        _isAtacking = true;
        Debug.Log($"{gameObject.name} deu soco esquerdo");
        ActiveHitBox(_leftPunchHitbox, _leftPunchDamage);
        Invoke("ResetAttack", _attackCooldown);
    }
    public void HighKick()
    {
        _isAtacking = true;
        Debug.Log($"{gameObject.name} deu um chute forte");
        ActiveHitBox(_hightKickHitbox, _highKickDamage);
        Invoke("ResetAttack", _attackCooldown);
    }

    public void LowKick()
    {
        _isAtacking = true;
        Debug.Log($"{gameObject.name} deu um chute fraco");
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
