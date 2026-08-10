using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Banco de Personagens")]
    [SerializeField] private CharacterData _characterData;

    [Header("Spawn do Player")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;

    [Header("Inimigo")]
    [SerializeField] private GameObject _enemyPrefab;

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
        if(_characterData == null)
        {
            Debug.LogError("CharacterSpawner: CharacterData não foi configurado");
            return;
        }

        if(_characterData._characters == null || 
        _characterData._characters.Length == 0)
        {
            Debug.LogError("CharacterSpawner: Nenhum Personagem Cadastrado");
            return;
        }

        int selectedID = CharacterSelect.SelectedCharacter;

        if(selectedID < 0 ||
        selectedID >= _characterData._characters.Length)
        {
            Debug.LogError($"CharacterSpawner: ID de personagem inválido: {selectedID}");
            return;
        }

        CharacterDataBase selectedCharacters = 
        _characterData._characters[selectedID];

        if(selectedCharacters.prefab == null)
        {
            Debug.LogError(
                $"CharacterSpawner: O prefab de {selectedCharacters.characterName} não foi configurado!"
            );

            return;
        }

        _spawnedPlayer = Instantiate(selectedCharacters.prefab,
        _playerSpawnPoint.position,
        _playerSpawnPoint.rotation);

        _spawnedPlayer.name = 
        selectedCharacters.characterName + "_Player";

        Debug.Log(
            $"Player Spawnado: {selectedCharacters.characterName}"
        );

        _spawnedEnemy = Instantiate(
            _enemyPrefab,
            _enemySpawnPoint.position,
            _enemySpawnPoint.rotation
        );

        _spawnedEnemy.name = "Enemy";

        SetupHealthUI();

        Debug.Log("Enemy Spawnado");

        EnemyIA enemyIA = 
        _spawnedEnemy.GetComponent<EnemyIA>();

        if(enemyIA != null)
        {
            enemyIA.SetPlayer(_spawnedPlayer.transform);
        }
        else
        {
            Debug.LogError(
                "CharacterSpawner: O Enemy Prefab não possui EnemyIA!"
            );
        }

        EnemyControllerFight enemyControllerFight =
            _spawnedEnemy.GetComponent<EnemyControllerFight>();

        if(enemyControllerFight != null)
        {
            enemyControllerFight.SetPlayer(_spawnedPlayer.transform);
        }
        else
        {
            Debug.LogError(
                "CharacterSpawner: Enemy não possui EnemyControllerFight");
        }


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

}
