using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Banco de Personagens")]
    [SerializeField] private CharacterData _characterData;

    [Header("Spawn do Player")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;


    [Header("CineMachine")]
    [SerializeField] private CinemachineTargetGroup _targetGroup;

    [Header("HUD de Vida")]
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private Slider _enemyHealthSlider;

    private GameObject _spawnedPlayer;
    private GameObject _spawnedEnemy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPlayer();
        SetupCamera();
    }

    private void SpawnPlayer()
    {
        if (_characterData == null)
        {
            Debug.LogError(
                "CharacterSpawner: CharacterData não configurado!"
            );

            return;
        }

        if (_characterData._characters == null ||
            _characterData._characters.Length == 0)
        {
            Debug.LogError(
                "CharacterSpawner: Nenhum personagem cadastrado!"
            );

            return;
        }


        // ==========================================
        // PLAYER
        // ==========================================

        int playerID = CharacterSelect.SelectedPlayerCharacter;

        if (playerID < 0 ||
            playerID >= _characterData._characters.Length)
        {
            Debug.LogError(
                $"ID do Player inválido: {playerID}"
            );

            return;
        }


        CharacterDataBase playerData =
            _characterData._characters[playerID];


        if (playerData.prefab == null)
        {
            Debug.LogError(
                $"Prefab do personagem {playerData.characterName} não configurado!"
            );

            return;
        }


        _spawnedPlayer = Instantiate(
            playerData.prefab,
            _playerSpawnPoint.position,
            _playerSpawnPoint.rotation
        );


        _spawnedPlayer.name =
            playerData.characterName + "_Player";

        CombatSide playerSide =
           _spawnedPlayer.GetComponent<CombatSide>();

        
        if(playerSide != null)
        {
            playerSide.SetSide(CombatSide.Side.Player);
        }
        else
        {
            Debug.LogError(
        $"CharacterSpawner: {playerData.characterName} não possui CombatSide!"
    );
        }


        Debug.Log(
            $"PLAYER: {playerData.characterName}"
        );


        // ==========================================
        // ENEMY / CPU
        // ==========================================

        int enemyID = CharacterSelect.SelectedEnemyCharacter;


        if (enemyID < 0 ||
            enemyID >= _characterData._characters.Length)
        {
            Debug.LogError(
                $"ID do Enemy inválido: {enemyID}"
            );

            return;
        }


        CharacterDataBase enemyData =
            _characterData._characters[enemyID];


        if (enemyData.prefab == null)
        {
            Debug.LogError(
                $"Prefab do personagem {enemyData.characterName} não configurado!"
            );

            return;
        }


        _spawnedEnemy = Instantiate(
            enemyData.prefab,
            _enemySpawnPoint.position,
            _enemySpawnPoint.rotation
        );


        _spawnedEnemy.name =
            enemyData.characterName + "_CPU";

        ConfigureCPU(_spawnedEnemy);


        CombatSide enemySide =
            _spawnedEnemy.GetComponent<CombatSide>();
        
        if(enemySide != null)
        {
            enemySide.SetSide(CombatSide.Side.Enemy);
        }

        else
        {
          Debug.LogError(
        $"CharacterSpawner: {enemyData.characterName} não possui CombatSide!"
    );  
        }


        Debug.Log(
            $"CPU: {enemyData.characterName}"
        );


        // ==========================================
        // CONFIGURA IA
        // ==========================================

        EnemyIA enemyIA =
            _spawnedEnemy.GetComponent<EnemyIA>();


        if (enemyIA != null)
        {
            enemyIA.SetPlayer(
                _spawnedPlayer.transform
            );
        }
        else
        {
            Debug.LogError(
                "CharacterSpawner: O personagem escolhido para CPU não possui EnemyIA!"
            );
        }

        EnemyControllerFight enemyFight =
            _spawnedEnemy.GetComponent<EnemyControllerFight>();
        
        if (enemyFight != null)
        {
            enemyFight.SetPlayer(
                _spawnedPlayer.transform
            );
        }
        else
        {
            Debug.LogWarning(
        "CharacterSpawner: CPU não possui EnemyControllerFight."
    );
        }


        NewPlayMove playerMove =
    _spawnedPlayer.GetComponent<NewPlayMove>();

NewPlayMove enemyMove =
    _spawnedEnemy.GetComponent<NewPlayMove>();

if (playerMove != null)
{
    playerMove.SetEnemy(_spawnedEnemy.transform);
}

if (enemyMove != null)
{
    enemyMove.SetEnemy(_spawnedPlayer.transform);
}


        // ==========================================
        // HEALTH
        // ==========================================

        SetupHealthUI();


    }

    private void SetupCamera()
    {
        if (_targetGroup == null)
    {
        Debug.LogWarning(
            "CharacterSpawner: TargetGroup não foi configurado."
        );

        return;
    }

    // Limpa os personagens que estavam configurados anteriormente
    _targetGroup.Targets.Clear();

    // Adiciona o Player escolhido
    _targetGroup.Targets.Add(
        new CinemachineTargetGroup.Target
        {
            Object = _spawnedPlayer.transform,
            Weight = 1f,
            Radius = 1f
        }
    );

    // Adiciona o Enemy
    _targetGroup.Targets.Add(
        new CinemachineTargetGroup.Target
        {
            Object = _spawnedEnemy.transform,
            Weight = 1f,
            Radius = 1f
        }
    );

    Debug.Log("Cinemachine TargetGroup configurado com Player + Enemy!");
}

private void SetupHealthUI()
    {
        if (_spawnedPlayer != null)
    {
        HealthForAll playerHealth =
            _spawnedPlayer.GetComponent<HealthForAll>();

        if (playerHealth != null)
        {
            playerHealth.SetHealthSlider(_playerHealthSlider);
        }
        else
        {
            Debug.LogError(
                "CharacterSpawner: Player não possui HealthForAll!"
            );
        }
    }

    if (_spawnedEnemy != null)
    {
        HealthForAll enemyHealth =
            _spawnedEnemy.GetComponent<HealthForAll>();

        if (enemyHealth != null)
        {
            enemyHealth.SetHealthSlider(_enemyHealthSlider);
        }
        else
        {
            Debug.LogError(
                "CharacterSpawner: Enemy não possui HealthForAll!"
            );
        }
    }
    
    }

    private void ConfigureCPU(GameObject cpu)
    {
       EnemyIA enemyIA = cpu.GetComponent<EnemyIA>();

    if (enemyIA == null)
    {
        Debug.LogError(
            $"CharacterSpawner: {cpu.name} não possui EnemyIA!"
        );

        return;
    }

    enemyIA.SetControlMode(
        EnemyIA.ControlMode.AI
    );

    enemyIA.SetControlledByEnemyIA(true);

    PlayerInput playerInput =
        cpu.GetComponent<PlayerInput>();

    if (playerInput != null)
    {
        playerInput.enabled = false;
    }

    Debug.Log(
        $"{cpu.name} configurado como CPU."
    ); 
    }

}
