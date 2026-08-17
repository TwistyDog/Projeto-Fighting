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

    [SerializeField] private Animator _animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        if(_animator == null)
           _animator = GetComponentInChildren<Animator>();

        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Combat");
    }

    //

    public void RightPuch()
    {
        TryAttack(_rightPunchHitbox, _rightPunchDamage, "WeakPunch");
        
    }

    public void LeftPuch()
    {
        TryAttack(_leftPunchHitbox, _leftPunchDamage, "StrongPunch");
    }
    public void HighKick()
    {
        TryAttack(_hightKickHitbox, _highKickDamage, "HighKick");
    }

    public void LowKick()
    {
        TryAttack(_lowKickHitbox, _lowKickDamage, "lowKick");
    }

    void TryAttack(GameObject hitbox, int damage, string animationSetTrigger)
    {
        if(_isAtacking) return;

        if (hitbox == null)
    {
        Debug.LogWarning($"{gameObject.name} tentou usar um hitbox que não está configurado!");
        return;
    }
    _isAtacking = true;

    if(_animator != null)
        {
            _animator.SetTrigger(animationSetTrigger);
        }

    HitBox hb = hitbox.GetComponent<HitBox>();

    if(hb != null)
        {
            string targetTag = CompareTag("Player") ? "Enemy" : "Player";

            hb.Setup(damage, targetTag);
        }

    hitbox.SetActive(true);

    Invoke(nameof(DisableHitBox), 0.1f);
    Invoke(nameof(ResetAttack), _attackCooldown);

    }

    void DisableHitBox()
    {
        _rightPunchHitbox?.SetActive(false);
        _leftPunchHitbox?.SetActive(false);
        _hightKickHitbox?.SetActive(false);
        _lowKickHitbox?.SetActive(false);
    }

    void ResetAttack() => _isAtacking = false;
    
}
