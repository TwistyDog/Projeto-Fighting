using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightCombat : MonoBehaviour
{
    private bool _isAtacking = false;

    private Coroutine _attackLockCoroutine;

    [Header("HitBoxes")]
    [SerializeField] private GameObject _rightPunchHitbox;
    [SerializeField] private GameObject _leftPunchHitbox;
    [SerializeField] private GameObject _hightKickHitbox;
    [SerializeField] private GameObject _lowKickHitbox;

    [Header("Damage")]
    [SerializeField] private int _rightPunchDamage = 10;
    [SerializeField] private int _leftPunchDamage = 12;
    [SerializeField] private int _highKickDamage = 15;
    [SerializeField] private int _lowKickDamage = 8;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    public bool IsAtacking => _isAtacking;

    private CombatSide _combatSide;


    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();

        _combatSide = GetComponent<CombatSide>();

        PlayerInput playerInput =
            GetComponent<PlayerInput>();

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Combat");

        // Garante que as hitboxes começam desligadas
        DisableHitBox();
    }


    // =========================================================
    // ATAQUES
    // =========================================================

    public void RightPuch()
    {
        TryAttack(
            _rightPunchHitbox,
            _rightPunchDamage,
            "WeakPunch"
        );
    }


    public void LeftPuch()
    {
        TryAttack(
            _leftPunchHitbox,
            _leftPunchDamage,
            "StrongPunch"
        );
    }


    public void HighKick()
    {
        TryAttack(
            _hightKickHitbox,
            _highKickDamage,
            "HighKick"
        );
    }


    public void LowKick()
    {
        TryAttack(
            _lowKickHitbox,
            _lowKickDamage,
            "lowKick"
        );
    }


    // =========================================================
    // EXECUTA ATAQUE
    // =========================================================

    private void TryAttack(
        GameObject hitbox,
        int damage,
        string animationSetTrigger)
    {
        // =====================================================
        // JÁ ESTÁ ATACANDO
        // =====================================================

        if (_isAtacking)
            return;


        if (hitbox == null)
        {
            Debug.LogWarning(
                $"{gameObject.name} tentou usar um hitbox que não está configurado!"
            );

            return;
        }


        if (_combatSide == null)
        {
            Debug.LogError(
                $"{gameObject.name} não possui CombatSide!"
            );

            return;
        }


        // =====================================================
        // BLOQUEIA NOVOS ATAQUES
        // =====================================================

        _isAtacking = true;


        // =====================================================
        // ANIMAÇÃO
        // =====================================================

        if (_animator != null)
        {
            _animator.SetTrigger(
                animationSetTrigger
            );
        }


        // =====================================================
        // CONFIGURA HITBOX
        // =====================================================

        HitBox hb =
            hitbox.GetComponent<HitBox>();


        if (hb != null)
        {
            CombatSide.Side targetSide =
                _combatSide.CurrentSide ==
                CombatSide.Side.Player

                ? CombatSide.Side.Enemy
                : CombatSide.Side.Player;


            hb.Setup(
                damage,
                targetSide
            );
        }


        // =====================================================
        // ATIVA HITBOX
        // =====================================================

        DisableHitBox();

        hitbox.SetActive(true);


        Invoke(
            nameof(DisableHitBox),
            0.1f
        );

        if (_attackLockCoroutine != null)
            StopCoroutine(_attackLockCoroutine);

        _attackLockCoroutine =
            StartCoroutine(WaitForAttackAnimation());
    }


    // =========================================================
    // DESATIVA HITBOX
    // =========================================================

    private IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitUntil(() =>
        _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));

        yield return new WaitUntil(() =>
        !_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));

        _isAtacking = false;

        _attackLockCoroutine = null;

        Debug.Log(
        $"{gameObject.name}: ataque finalizado!"
    );
    }

    private void DisableHitBox()
    {
        _rightPunchHitbox?.SetActive(false);
        _leftPunchHitbox?.SetActive(false);
        _hightKickHitbox?.SetActive(false);
        _lowKickHitbox?.SetActive(false);
    }


    // =========================================================
    // FINAL DA ANIMAÇÃO
    // CHAMADO PELO ANIMATION EVENT
    // =========================================================

}
