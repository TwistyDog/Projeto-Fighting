using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected Vector2 _moveInput;
    [SerializeField] protected Vector3 _playerVelocity;
    [SerializeField] protected bool _groundedPlayer;
    [SerializeField] protected float _playerSpeed = 6.0f;
    [SerializeField] protected float _jumpHeight = 3.0f;
    [SerializeField] protected float _gravityValue = -9.81f;
    [SerializeField] protected float _crouchHeight = 0.5f;

    private protected float _originalHeight;
    [SerializeField] protected bool _isCrounching = false;

    [SerializeField] protected bool _isBlocking = false;
    [SerializeField] protected float _damageReduction = 0.5f;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        _controller = GetComponent<CharacterController>();
        _originalHeight = _controller.height;

    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        Gravity();

        Move();
       // Jump();
        Crouch();

       
    }


    protected virtual void Move()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(_moveInput.x, 0, 0);

        if (move.x != 0)
        {
            float targetAngle = move.x > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0f);
        }

        _controller.Move(move * _playerSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_groundedPlayer == true)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -1.0f * _gravityValue);    
        }
    }

    protected virtual void Gravity()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -3f;
        }
        
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    

    protected virtual void Crouch()
    {
        if (_groundedPlayer && Input.GetAxisRaw("Vertical") < -0.5f)
        {
            _isCrounching = true;
            _controller.height = _originalHeight;
        }
        else if (_isCrounching)
        {
            _isCrounching = false;
            _controller.height = _originalHeight;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    protected virtual void CheckBlock()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        _isBlocking = _groundedPlayer &&
                                     ((transform.forward.x > 0 && horizontalInput < -0.5f) ||
                                     (transform.forward.x < 0 && horizontalInput > -0.5f));
        Debug.Log("defesa");


    }

    protected virtual void TakeDamage(float damage)
    {
        float finalDamage = _isBlocking ? damage * _damageReduction : damage;
        Debug.Log($"Dano Recebido: {finalDamage}(Bloqueado: {_isBlocking})");
    }
    
    protected virtual void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
}

