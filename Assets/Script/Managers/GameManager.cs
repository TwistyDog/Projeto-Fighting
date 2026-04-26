using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool podeControlar = false;
    void Awake()
    {
        Instance = this;
    }

    public void LiberarControle()
    {
        podeControlar = true;
    }

    public void TravarControle()
    {
        podeControlar = false;
    }
}
