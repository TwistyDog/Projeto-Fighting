using UnityEngine;

public enum CharactherControllerType
{
    Player,
    CPU
}
[System.Serializable]

public class CharacterDataBase
{
    public string characterName;
    public GameObject prefab;
    public Sprite portrait;
    
}
