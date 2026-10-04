using UnityEngine;

public class CombatSide : MonoBehaviour
{
    public enum Side
    {
        Player,
        Enemy
    }

    [SerializeField] private Side _side;

    public Side CurrentSide => _side;

    public void SetSide(Side side)
    {
        _side = side;
    }

    public bool IsPlayer()
    {
        return _side == Side.Player;
    }
}
