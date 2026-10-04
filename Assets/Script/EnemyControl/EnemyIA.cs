

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using SRandom = System.Random;
using URandom = System.Random;
using UnityEngine.InputSystem;



public class EnemyIA : NewPlayMove
{

    private SpecialMoves1 _specialMoves1;
    private SpecialMove2 _specialMoves2;

    public enum SpeciaMoveType
    {
        None,
        SpecialMoves1,
        SpecialMove2
    }

    private SpeciaMoveType _specialMoveType =
          SpeciaMoveType.None;
    public enum ControlMode
    {
        AI,
        Player
    }

    [Header("Controle")]
    [SerializeField]
    private ControlMode _controlMode = ControlMode.AI;
    
    [SerializeField] private Transform _player;
    [SerializeField] public NewPlayMove moverPlayer;   
    [SerializeField] private float _stoppingDistance = 2f;

    [Header("Arena Limites")]
    [SerializeField] private float _arenaMinX = -8f;
    [SerializeField] private float _arenaManX = 8f;

    [Header("Configuração de Movimento")]
    [SerializeField] private float _changeStateTime = 2f;
    [SerializeField] private float _retreatDistance = 3f;
    [SerializeField] private float _forwardDistance = 5f;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _retreatStep = 2f;
    

    [Header("Reação ao Pulo do Player")]
    [SerializeField, Range(0f, 1f)] private float _jumpReactionChance = 0.6f;
    [SerializeField] private float _minReactDistance = 0f;

    [Header("Cooldown de Pulo")]
    [SerializeField] private float _jumpCoolDown = 2f;
    [SerializeField] private float _minJumpDistance = 1.5f;
    [SerializeField] private float _maxJumpDistance = 6f;

    private float _jumpTimer;

    private float _startTime;
    private EnemyState _currentState;

    private SpecialMoves1 _playerMovei;

    private bool _playerWasGrounded;

    private enum EnemyState { Idle, Advance, Retreat, Chase }

    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundedBufferTime = 0.1f;
    [SerializeField] DamageReceiver _damageReceiver;


    private float _groundedTimer;

    protected void Awake()
    {
        _damageReceiver =
           GetComponent<DamageReceiver>();

           _specialMoves1 =
            GetComponent<SpecialMoves1>();

            _specialMoves2 = 
            GetComponent<SpecialMove2>();

        DetectarSpecialMove();

        ChangeState();
    }

    
    private void DetectarSpecialMove()
    {
        if (_specialMoves1 != null)
        {
            _specialMoveType =
                SpeciaMoveType.SpecialMoves1;

            Debug.Log(
                gameObject.name +
                " detectou SpecialMoves1."
            );

            return;
        }


        if (_specialMoves2 != null)
        {
            _specialMoveType =
                SpeciaMoveType.SpecialMove2;

            Debug.Log(
                gameObject.name +
                " detectou SpecialMove2."
            );

            return;
        }


        _specialMoveType =
            SpeciaMoveType.None;


        Debug.Log(
            gameObject.name +
            " não possui SpecialMoves1 nem SpecialMove2."
        );
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (_controller == null || !_controller.enabled)
        return;

    if (!GameManager.Instance.podeControlar)
        return;

    if (_controlMode == ControlMode.AI)
    {
        UpdateAI();
    }
    else
    {
        UpdatePlayer();
    }

    }

    private void UpdateAI()
    {
        _jumpTimer -= Time.deltaTime;

    if (_groundedPlayer && _playerVelocity.y < 0)
    {
        _playerVelocity.y = -0.5f;
    }

    _playerVelocity.y += _gravityValue * Time.deltaTime;

    _startTime -= Time.deltaTime;

    if (_startTime <= 0)
    {
        ChangeState();
    }

    if (_player != null)
    {
        if (_player.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    HandleMovement();
    HandleJumpReaction();

    UptadeGroundedStable();
    AlignWithPlayer();
    }


    private void UpdatePlayer()
    {
        if (_controller == null || !_controller.enabled)
        return;

    if (_groundedPlayer && _playerVelocity.y < 0)
    {
        _playerVelocity.y = -0.5f;
    }

    _playerVelocity.y += _gravityValue * Time.deltaTime;

    HandlePlayerMovement();

    UptadeGroundedStable();
    }





    protected virtual void ChangeState()
    {
        int rand = Random.Range(0, 4);
        _currentState = (EnemyState)rand;
        _startTime = Random.Range(_changeStateTime * 0.7f, _changeStateTime * 1.3f);
    }

    protected void HandleMovement()
    {
        if(_controller == null || !_controller.enabled)
           return;

        if (_player == null) return;

        float desiredX = transform.position.x;

        switch (_currentState)
        {
            case EnemyState.Idle:
                break;

            case EnemyState.Advance:
                desiredX = _player.position.x;
                break;

            case EnemyState.Retreat:
                desiredX = transform.position.x + ((_player.position.x > transform.position.x) ? -_retreatStep : _retreatStep);
                break;

            case EnemyState.Chase:
                desiredX = _player.position.x;
                break;
        }

        desiredX = Mathf.Clamp(desiredX, _arenaMinX, _arenaManX);

        //float distance = Mathf.Abs(_player.position.x - transform.position.x);

        float  newX = Mathf.MoveTowards(transform.position.x, desiredX, _moveSpeed * Time.deltaTime);
        float moveX = newX - transform.position.x;

        float moveY = _playerVelocity.y * Time.deltaTime;
        

        Vector3 finalMove = new Vector3(moveX, moveY, 0);

        _controller.Move(finalMove);
            
        }


        private void HandlePlayerMovement()
    {
        float moveX = _moveInput.x;

    Vector3 movement =
        transform.right *
        moveX *
        _moveSpeed *
        Time.deltaTime;

    movement.y =
        _playerVelocity.y *
        Time.deltaTime;

    _controller.Move(movement);

    if (Mathf.Abs(moveX) > 0.01f)
    {
        if (moveX > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);
    }
    }

    private void HandleJumpReaction()
    {
        if (_player == null) return;

    bool playerGrounded =
        (_playerMovei != null) ? _playerMovei._groundedPlayer :
        (_player.TryGetComponent(out CharacterController pc) ? pc.isGrounded : true);

    float distance = Mathf.Abs(_player.position.x - transform.position.x);

    // 👇 só reage quando player acabou de pular
    if (_playerWasGrounded && !playerGrounded)
    {
        // 👇 condições inteligentes
        bool withinDistance = distance >= _minJumpDistance && distance <= _maxJumpDistance;
        bool canJump = _jumpTimer <= 0f;
        bool randomPass = Random.value < _jumpReactionChance;

        if (_groundedPlayer && withinDistance && canJump && randomPass)
        {
            JumpAI();

            // 👇 reseta cooldown
            _jumpTimer = _jumpCoolDown;
        }
    }

    _playerWasGrounded = playerGrounded;
    }

    private bool GetPlayerGrounded()
    {
       if (_player == null)
            return true;


        // Primeiro tenta SpecialMoves1
        SpecialMoves1 special1 =
            _player.GetComponent<SpecialMoves1>();


        if (special1 != null)
        {
            return special1._groundedPlayer;
        }


        // Depois tenta SpecialMove2
        SpecialMove2 special2 =
            _player.GetComponent<SpecialMove2>();


        if (special2 != null)
        {
            return special2._groundedPlayer;
        }


        // Último fallback
        CharacterController controller =
            _player.GetComponent<CharacterController>();


        if (controller != null)
        {
            return controller.isGrounded;
        }


        return true; 
    }


     public void JumpAI()
    {
        if (_groundedPlayer && !_isCrouching)
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravityValue);
    }

    public void ResetStateEnemy()
    {
        _playerVelocity = Vector3.zero;
        _isCrouching = false;
        _jumpTimer = 0f;
    }


    public void BlockAI(bool block)
    {
        _isBlocking = block;

        if(_damageReceiver != null)
           _damageReceiver.SetBlocking(block);
    }


    void UptadeGroundedStable()
    {
        bool controllerGrounded = _controller.isGrounded;

    Vector3 origin = transform.position + Vector3.up * 0.1f;
    bool rayGrounded = Physics.Raycast(origin, Vector3.down, _groundCheckDistance, _groundLayer);

    if (controllerGrounded || rayGrounded)
    {
        _groundedTimer = _groundedBufferTime;
    }
    else
    {
        _groundedTimer -= Time.deltaTime;
    }

    _groundedPlayer = _groundedTimer > 0f;
    }

    void AlignWithPlayer()
    {
        if(_player == null) return;

        Vector3 pos = transform.position;

        pos.z = Mathf.Lerp(pos.z, _player.position.z, 10f * Time.deltaTime);

        transform.position = pos;
    }
    

    public void SetPlayer(Transform player)
    {
        _player = player;

        if(_player != null)
        {
            _playerMovei = _player.GetComponent<SpecialMoves1>();

            Debug.Log(
            "EnemyIA recebeu o Player: " + _player.name
        );

        }
    }

    public void SetSpecialMoveType(SpeciaMoveType type)
    {
        _specialMoveType =
            type;


        Debug.Log(
            gameObject.name +
            " recebeu SpecialMoveType: " +
            type
        );
    }

    public SpeciaMoveType GetSpecialMoveType()
    {
        return _specialMoveType;
    }


    public SpecialMoves1 GetSpecialMoves1()
    {
        return _specialMoves1;
    }


    public SpecialMove2 GetSpecialMove2()
    {
        return _specialMoves2;
    }


    public void OnnMove(InputAction.CallbackContext context)
    {
        if(_controlMode != ControlMode.Player)
           return;
        
        base.OnMove(context);
    }

    public void Onjuump(InputAction.CallbackContext context)
    {
       if (_controlMode != ControlMode.Player)
        return;

    if (!context.performed)
        return;

       base.OnJump(context);
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(_controlMode != ControlMode.Player)
           return;
        
        BlockAI(context.ReadValueAsButton());
    }

    public void SetControlMode(ControlMode mode)
    {
        _controlMode = mode;

        SetControlledByEnemyIA(mode == ControlMode.AI);

        Debug.Log(
        gameObject.name +
        " modo de controle: " +
        _controlMode
    );
    }
   

    }
    
    
