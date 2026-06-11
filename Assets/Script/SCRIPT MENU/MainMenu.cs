using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nomeCenaJogo = "Area de Rua";
    public string nomeOpcoes = "OptionScene";

    [Header("Paineis")]
    [SerializeField] GameObject _mainMenuPainel;
    [SerializeField] GameObject _characterSelectPanel;

    
    
    public void NovoJogo()
    {
        _mainMenuPainel.SetActive(false);
        _characterSelectPanel.SetActive(true);
    }

    public void VoltarMenu()
    {
        _characterSelectPanel.SetActive(false);
        _mainMenuPainel.SetActive(true);
    }

    public void Opcoes()
    {
        SceneManager.LoadScene(nomeOpcoes);
    }

    public void Sair()
    {
        Debug.Log("Saindo do Jogo");
        Application.Quit();
    }

}
