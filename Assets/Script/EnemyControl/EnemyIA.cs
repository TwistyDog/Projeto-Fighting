
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class EnemyIA : SpecialMove2
{
    
    [SerializeField] private Transform _player;
   
    [SerializeField] private float _stoppingDistance = 2f;

    [Header("Arena Limites")]
    [SerializeField] private float _arenaMinX = -8f;
    [SerializeField] private float _arenaManX = 8f;

    [Header("Configuração de Movimento")]
    [SerializeField] private float _changeStateTime = 2f;
    [SerializeField] private float _retreatDistance = 3f;
    [SerializeField] private float _forwardDistance = 5f;
    [SerializeField] private float _moveSpeed = 3f;

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
            _playerMovei = _player.GetComponent<SpecialMoves1>();
        }

    }

    // Update is called once per frame
    protected override void Update()
    {


        if (_player == null) return;

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
        

    }



    protected virtual void ChangeState()
    {
        int rand = Random.Range(0, 4);
        _currentState = (EnemyState)rand;
        _startTime = Random.Range(_changeStateTime * 0.7f, _changeStateTime * 1.3f);
    }

    protected void HandleMovement()
    {
        float targetX = transform.position.x;

        switch (_currentState)
        {
            case EnemyState.Idle:
                return;

            case EnemyState.Advance:
                targetX = Mathf.MoveTowards(transform.position.x, _player.position.x, _moveSpeed * Time.deltaTime);
                break;

            case EnemyState.Retreat:

                if (_player.position.x > transform.position.x)
                    targetX = transform.position.x - (_moveSpeed * Time.deltaTime);
                else
                    targetX = transform.position.x + (_moveSpeed * Time.deltaTime);
                break;

            case EnemyState.Chase:

                targetX = Mathf.MoveTowards(transform.position.x, _player.position.x, _moveSpeed * Time.deltaTime);
                break;
        }

        targetX = Mathf.Clamp(targetX, _arenaMinX, _arenaManX);

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        //_agent.SetDestination(targetPosition);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 velocity = direction * _moveSpeed;

        //_playerVelocity.y += _gravityValue * Time.deltaTime;

        //Vector3 finalMove = new Vector3(horizontal.x, _playerVelocity.y, horizontal.z);

        _controller.Move(velocity * Time.deltaTime);
    }

   

    private void HandleJumpReaction()
    {
        if (_playerMovei != null && _playerMovei.IsGrounded == false && _groundedPlayer)
        {
            if(Random.value > 0.5f)
               JumpAI();
            
            
        }

    }

    public void JumpAI()
    {
        if(_groundedPlayer && !_isCrouching)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
        }
    } 

    public void CrouchAI(bool crouch)
    {
        if (crouch && !_isCrouching)
        {
            _isCrouching = true;
            _controller.height = _crouchHeight;
            Vector3 center = _controller.center;
            center.y = _controller.height / 2f;
            _controller.center = center;
            transform.position += new Vector3(0, (_crouchHeight - _originalHeight) / 2f, 0);
        }
    }

    public void BlockAI(bool block)
    {
        _isBlocking = block;
    }
}
