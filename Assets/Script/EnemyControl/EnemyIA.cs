

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using SRandom = System.Random;
using URandom = System.Random;



public class EnemyIA : SpecialMoves1
{
    
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
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _retreatStep = 2f;

    [Header("Reação ao Pulo do Player")]
    [SerializeField, Range(0f, 1f)] private float _jumpReactionChance = 0.6f;
    [SerializeField] private float _minReactDistance = 0f;

    private float _startTime;
    private EnemyState _currentState;

    private SpecialMoves1 _playerMovei;

    private bool _playerWasGrounded;

    private enum EnemyState { Idle, Advance, Retreat, Chase }

    protected override void Awake()
    {

        base.Awake();
        

        ChangeState();

        if (_player != null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) _player = go.transform;
        }

        if (_player == null)
        {
            _playerMovei = _player.GetComponent<SpecialMoves1>();
            if(_playerMovei == null)
            {
                Debug.LogWarning("EnemyIA: o Player não tem PlayerMove (base) anexado. Exponha GroundePlayer no Script dele ou use fallback por CharacterController");
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0f)
            _playerVelocity.y = 0f;

       // if (_player == null) return;

        _startTime -= Time.deltaTime;
        if (_startTime <= 0)
        {
            ChangeState();
        }


        if (_player.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);

        HandleMovement();
        //HandleIA();
        HandleJumpReaction();

        _playerVelocity.y += _gravityValue * Time.deltaTime;

        _controller.Move(new Vector3(0f, _playerVelocity.y, 0f) * Time.deltaTime);
    }



    protected virtual void ChangeState()
    {
        int rand = Random.Range(0, 4);
        _currentState = (EnemyState)rand;
        _startTime = Random.Range(_changeStateTime * 0.7f, _changeStateTime * 1.3f);
    }

    protected void HandleMovement()
    {
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

        float  newX = Mathf.MoveTowards(transform.position.x, desiredX, _moveSpeed * Time.deltaTime);

        Vector3 dx = new Vector3(newX - transform.position.x, 0f, 0f);
        if(dx.sqrMagnitude > 0f)
            _controller.Move(dx);
        }
        //

    private void HandleJumpReaction()
    {
        if (_player == null) return;

        bool playerGrounded =
            (_playerMovei != null) ? _playerMovei._groundedPlayer :

            (_player.TryGetComponent(out CharacterController pc) ? pc.isGrounded : true);

        if (_playerWasGrounded && !playerGrounded)
        {

            if (_minReactDistance <= 0f || Mathf.Abs(_player.position.x - transform.position.x) <= _minReactDistance)
            {
                if (_groundedPlayer && Random.value < _jumpReactionChance)
                {
                    JumpAI();
                }
            }
        }

        _playerWasGrounded = playerGrounded;
    }


     public void JumpAI()
    {
        if (_groundedPlayer && !_isCrouching)
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravityValue);
    }

    public void CrouchIA(bool crouch)
    {
        if (crouch && !_isCrouching)
        {
            _isCrouching = true;
            _controller.height = _crouchHeight;
            var c = _controller.center; c.y = _crouchHeight / 2f; _controller.center = c;
            transform.position += new Vector3(0, (_originalHeight - _crouchHeight) / 2f, 0);
        }
        else if (!crouch && _isCrouching)
        {
            _isCrouching = false;
            _controller.height = _originalHeight;
            var c = _controller.center; c.y = _originalHeight / 2f; _controller.center = c;
            transform.position += new Vector3(0, (_crouchHeight - _originalHeight) / 2f, 0);
        }
    }

    public void BlockAI(bool block) => _isBlocking = block;
   

    }
    
    
