using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{

    public static int SelectedCharacter;

    [SerializeField] private LoadingManager loadingManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SelectCharacter(int id)
    {
        SelectedCharacter = id;

        loadingManager.StartLoading("Area de Rua");
    }
}
