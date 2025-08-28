using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayMove : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected CharacterController _controller;

    [Header("Movement")]
    [SerializeField] protected float _playerSpeed = 6f;
    [SerializeField] protected float _jumpHeight = 5f;
    [SerializeField] protected float _gravityValue = -9.81f;
    [SerializeField] protected float _crouchHeight = 0.5f;

    [Header("Combat")]
    [SerializeField] protected bool _isBlocking = false;
    [SerializeField] protected float _damageReduction = 0.5f;

    protected Vector3 _playerVelocity;
    [SerializeField] public bool _groundedPlayer;
    protected float _originalHeight;
    protected bool _isCrouching = false;

    protected virtual void Start()
    {
        _controller = GetComponent<CharacterController>();
        _originalHeight = _controller.height;
    }

    protected virtual void Update()
    {
        _groundedPlayer = _controller.isGrounded;

        HandleCrouch();
        HandleMove();
    }

    #region Input Actions

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _playerVelocity.x = input.x * _playerSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _groundedPlayer && !_isCrouching)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
        }
    }

    #endregion

    #region Movement

    protected virtual void HandleMove()
    {
        // Aplica gravidade
        if (_groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        _playerVelocity.y += _gravityValue * Time.deltaTime;

        // Rotação do personagem
        if (_playerVelocity.x != 0)
        {
            float targetAngle = _playerVelocity.x > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }

        // Move o CharacterController
        Vector3 move = new Vector3(_playerVelocity.x, _playerVelocity.y, 0);
        _controller.Move(move * Time.deltaTime);
    }

    protected virtual void HandleCrouch()
    {
        if (_groundedPlayer)
        {
            if (Input.GetAxisRaw("Vertical") < -0.5f)
            {
                _isCrouching = true;
                _controller.height = _crouchHeight;
                Vector3 center = _controller.center;
                center.y = _crouchHeight / 2f;
                _controller.center = center;
            }
            else if (_isCrouching)
            {
                _isCrouching = false;
                _controller.height = _originalHeight;
                Vector3 center = _controller.center;
                center.y = _originalHeight / 2f;
                _controller.center = center;
            }
        }
    }

    #endregion

    #region Combat

    protected virtual void CheckBlock()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        _isBlocking = _groundedPlayer &&
                      ((transform.forward.x > 0 && horizontalInput < -0.5f) ||
                       (transform.forward.x < 0 && horizontalInput > -0.5f));
    }

    protected virtual void TakeDamage(float damage)
    {
        float finalDamage = _isBlocking ? damage * _damageReduction : damage;
        Debug.Log($"Dano Recebido: {finalDamage} (Bloqueado: {_isBlocking})");
    }

    #endregion
}

