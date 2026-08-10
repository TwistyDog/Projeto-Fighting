using UnityEngine;

[RequireComponent(typeof(FightCombat))]
public class EnemyControllerFight : MonoBehaviour
{
    private FightCombat _combat;
    [SerializeField] private Transform _player;


    [SerializeField] private float _attackRange = 2f; // distancia minima para atacar
    [SerializeField] private float _minAttackDelay = 1f; // tempo minimo entre ataques
    [SerializeField] private float _maxAttackDelay = 2.5f; // tempo maximo entre ataques


    private float _nextAttackTime;

    private void Awake()
    {
        _combat = GetComponent<FightCombat>();
    }
    void Start()
    {
        _nextAttackTime = 
            Time.time + Random.Range(
                _minAttackDelay,
                _maxAttackDelay );
    }

    // Update is called once per frame
    void Update()
    {

        if(!GameManager.Instance.podeControlar)
           return;
           
        if (_player == null) return;

        float distance = Mathf.Abs(
            transform.position.x - _player.position.x);

        if (distance <= _attackRange && Time.time >= _nextAttackTime)
        {
            DoRandomAttack();
            _nextAttackTime = Time.time + Random.Range(_minAttackDelay, _maxAttackDelay);
        }
    }

    public void SetPlayer(Transform player)
    {
        _player = player;

        if(_player != null)
        {
            Debug.Log(
                "EnemyFightController recebeu o Player" + _player.name);
        }
    }

    private void DoRandomAttack()
    {

        if (_combat == null) return;

        if (_combat.IsAtacking) return; // n�o ataca enquanto estiver animando outro golpe

        int randomAttack = Random.Range(0, 4);
        switch (randomAttack)
        {
            case 0: Debug.Log ("Inimigo deu SOCO DIREITO"); _combat.RightPuch(); break;
            case 1: Debug.Log("Inimigo deu SOCO ESQUERDO");  _combat.LeftPuch(); break;
            case 2: Debug.Log("Inimigo deu CHUTE ALTO");  _combat.HighKick(); break;
            case 3: Debug.Log("Inimigo deu CHUTE BAIXO");  _combat.LowKick(); break;
        }
    }
}
