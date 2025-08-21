using UnityEngine;
using UnityEngine.AI;

public class NewEnemyIA : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _stoppingDistance = 1.5f;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.updatePosition = false;
        _agent.updateRotation = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_player != null) return;
        {
            Vector3 targetposition = new Vector3(_player.position.x, transform.position.y, transform.position.z);

            _agent.stoppingDistance = _stoppingDistance;
            _agent.SetDestination(targetposition);


            if (_player.position.x > transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);

            else
                transform.rotation = Quaternion.Euler(0,180,0);
        }
    }
}
