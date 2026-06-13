using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{

    public static int SelectedCharacter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SelectCharacter(int id)
    {
        SelectedCharacter = id;

        SceneManager.LoadScene("Area de Rua");
    }
}
