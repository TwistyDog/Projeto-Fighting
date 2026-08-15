
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayMove : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected CharacterController _controller;

    [SerializeField] private Transform _enemy;

    [Header("Movement")]
    [SerializeField] protected float _playerSpeed = 6f;
    [SerializeField] protected float _jumpHeight = 5f;
    [SerializeField] protected float _gravityValue = -9.81f;
    [SerializeField] protected float _crouchHeight = 0.5f;

    [Header("Combat")]
    [SerializeField] protected bool _isBlocking = false;
    [SerializeField] protected float _damageReduction = 0.5f;

    public Vector3 _playerVelocity;
    
    [SerializeField] public bool _groundedPlayer;

    protected Vector2 _moveInput;
    protected float _originalHeight;
    protected bool _isCrouching = false;

    protected virtual void Start()
    {
        _controller = GetComponent<CharacterController>();

        if(_controller == null)
        {
            Debug.LogError(
                $"{gameObject.name}: CharacterController não encontrado!"
            );

            return;
        }

        _originalHeight = _controller.height;

        if(_enemy == null)
        {
            GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");


            if(enemyObj != null)
               _enemy = enemyObj.transform;
        }
    }

    protected virtual void Update()
    {
        if (_controller == null || !_controller.enabled)
        return;

        if(!GameManager.Instance.podeControlar)
           return;

        _groundedPlayer = _controller.isGrounded;


        FaceEnemy();

        HandleCrouch();
        HandleMove();
        // Permite que classes derivadas executem lógica própria

        UpdateSpecialLogic();
    }

    protected virtual void UpdateSpecialLogic()
    {
        // Vazio no Player Normal
        //SpeciaMoves1 irá sobrescrever
    }

    #region Input Actions

    public virtual void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _playerVelocity.x = _moveInput.x * _playerSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed)
           return;
        
        if(!GameManager.Instance.podeControlar)
           return;
        
        if(_controller != null)
           _groundedPlayer = _controller.isGrounded;


        if (context.performed && _groundedPlayer && !_isCrouching)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);

            Debug.Log("Player Pulouu");
        }
    }

    public void ResetState()
    {
        _playerVelocity = Vector3.zero;
        _moveInput = Vector2.zero;
        _isCrouching = false;
    }

    #endregion

    #region Movement

    protected virtual void HandleMove()
    {
        
        if (_controller == null || !_controller.enabled)
        return;

        // Aplica gravidade
        if (_groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = -0.5f;

        _playerVelocity.y += _gravityValue * Time.deltaTime;


        // Move o CharacterController
        Vector3 move = new Vector3(_playerVelocity.x, _playerVelocity.y, 0);
        _controller.Move(move * Time.deltaTime);
    }

    protected virtual void FaceEnemy()
    {
        if(_enemy == null)
           return;
        
        if(_enemy.position.x > transform.position.x)
          transform.rotation = Quaternion.Euler(0f,0f,0f);
        
        else
          transform.rotation = Quaternion.Euler(0f,180f,0f);
    }

    protected virtual void HandleCrouch()
    {
        if (!_groundedPlayer)
            return;

        if (_moveInput.y < -0.5f)
        {
            if (!_isCrouching)
            {
                _isCrouching = true;

                _controller.height =
                    _crouchHeight;

                Vector3 center =
                    _controller.center;

                center.y =
                    _crouchHeight / 2f;

                _controller.center =
                    center;
            }
        }
        else if (_isCrouching)
        {
            _isCrouching = false;

            _controller.height =
                _originalHeight;

            Vector3 center =
                _controller.center;

            center.y =
                _originalHeight / 2f;

            _controller.center =
                center;
        }
    }

    #endregion

    #region Combat

    protected virtual void CheckBlock()
    {
        float horizontalInput = _moveInput.x;

        _isBlocking = _groundedPlayer &&
                      ((transform.forward.x > 0 && horizontalInput < -0.5f) ||
                       (transform.forward.x < 0 && horizontalInput > 0.5f));
    }

    protected virtual void TakeDamage(float damage)
    {
        float finalDamage = _isBlocking ? damage * _damageReduction : damage;
        Debug.Log($"Dano Recebido: {finalDamage} (Bloqueado: {_isBlocking})");
    }

    #endregion
}

