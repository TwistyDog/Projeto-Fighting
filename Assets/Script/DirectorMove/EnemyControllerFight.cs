using UnityEngine;

[RequireComponent(typeof(FightCombat))]
public class EnemyControllerFight : MonoBehaviour
{
    private FightCombat _combat;
    [SerializeField] private Transform _player;
    void Start()
    {
        _combat = GetComponent<FightCombat>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance < 2f && !_combat.IsAtacking)
        {
            int randomAttack = Random.Range(0, 4);
            switch (randomAttack)
            {
                case 0: _combat.RightPuch();break;
                case 1: _combat.LeftPuch(); break;
                case 2: _combat.HighKick(); break;
                case 3: _combat.LowKick();  break;   

            }
        }
    }
}
