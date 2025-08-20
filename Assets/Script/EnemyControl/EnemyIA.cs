using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private float _stoppingDistance = 2f;

    private float patrolTime;
    private Vector3 _patrolPoint;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null) return;

        Vector3 targetPosition = new Vector3(_player.position.x, transform.position.y, transform.position.z);
        
        float distance = Mathf.Abs(transform.position.x - _player.position.x);

        if (distance > _stoppingDistance)
        {
            _agent.SetDestination(targetPosition);
        }
        else
        {
            _agent.ResetPath();
        }

        if (_player.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0,180f,0);
    }
}
