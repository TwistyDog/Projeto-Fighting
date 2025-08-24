using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyIA : SpecialMove2
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private float _stoppingDistance = 2f;

    [Header("Arena Limites")]
    [SerializeField] private float _arenaMinX = -8f;
    [SerializeField] private float _arenaManX = 8f;

    [Header("Configura��o de Movimento")]
    [SerializeField] private float _changeStateTime = 2f;
    [SerializeField] private float _retreatDistance = 3f;
    [SerializeField] private float _forwardDistance = 5f;

    private float _startTime;
    private EnemyState _currentState;

    private enum EnemyState {Idle, Advance, Retreat, Chase}

    protected override void Awake()
    {

        base.Awake();
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.stoppingDistance = 0f;

        _agent.updatePosition = false;

        ChangeState();
        
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

        switch( _currentState )
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

        Vector3 direction = (_agent.nextPosition - transform.position).normalized;
        Vector3 velocity = direction * _agent.speed;

        _controller.Move(velocity * Time.deltaTime);
    }
}
