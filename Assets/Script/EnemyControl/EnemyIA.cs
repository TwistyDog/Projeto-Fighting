
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class EnemyIA : SpecialMove2
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private SpecialMove2 _playerMove;
    [SerializeField] private SpecialMoves1 _playerMovee;
    [SerializeField] private float _stoppingDistance = 2f;

    [Header("Arena Limites")]
    [SerializeField] private float _arenaMinX = -8f;
    [SerializeField] private float _arenaManX = 8f;

    [Header("Configuração de Movimento")]
    [SerializeField] private float _changeStateTime = 2f;
    [SerializeField] private float _retreatDistance = 3f;
    [SerializeField] private float _forwardDistance = 5f;

    private float _startTime;
    private EnemyState _currentState;


    private bool _playerWasGrounded;

    private enum EnemyState { Idle, Advance, Retreat, Chase }

    protected override void Awake()
    {

        base.Awake();
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.stoppingDistance = 0f;

        _agent.updatePosition = false;

        ChangeState();

        if (_player != null)
        {
            _playerMovee = _player.GetComponent<SpecialMoves1>();
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
        HandleIA();
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
                _agent.ResetPath();
                return;

            case EnemyState.Advance:
                targetX = Mathf.MoveTowards(transform.position.x, _player.position.x, 1f);
                break;

            case EnemyState.Retreat:

                if (_player.position.x > transform.position.x)
                    targetX = transform.position.x - 2f;
                else
                    targetX = transform.position.x + 2f;
                break;

            case EnemyState.Chase:

                targetX = _player.position.x;
                break;
        }

        targetX = Mathf.Clamp(targetX, _arenaMinX, _arenaManX);

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        _agent.SetDestination(targetPosition);

        Vector3 direction = (_agent.steeringTarget - transform.position).normalized;
        Vector3 horizontal = new Vector3(direction.x, 0, direction.z) * _agent.speed;

        _playerVelocity.y += _gravityValue * Time.deltaTime;

        Vector3 finalMove = new Vector3(horizontal.x, _playerVelocity.y, horizontal.z);

        _controller.Move(finalMove * Time.deltaTime);
    }

    protected void HandleIA()
    {
        float distance = Vector3.Distance(transform.position, _player.position);

        bool shouldCrouch = distance < 1.5f;

        CrouchIA(shouldCrouch);

        if (_groundedPlayer && Random.value < 0.09f)
        {
            JumpIA();
        }

        bool shoudBlock = distance < 2f && Random.value < 0.5f;
        BlockIA(shoudBlock);
    }

    private void HandleJumpReaction()
    {
        bool playerGround = _playerMovee.IsGrounded;
        if (_playerWasGrounded && !playerGround)
        {
            if (_groundedPlayer && Random.value < 0.6f)
            {
                JumpIA();
            }
        }
        _playerWasGrounded = playerGround;

    }

     
}
