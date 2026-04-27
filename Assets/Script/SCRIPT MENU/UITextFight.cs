using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITextFight : MonoBehaviour
{
    public static UITextFight instance;

    public TextMeshProUGUI _texto;
    public float tempoEntreTextos = 1.5f;

    private int roundAtual = 1;
    private bool lutaAtiva = false;

    public int playerWins = 0;
    public int enemyWins = 0;

    [SerializeField] private int maxRounds = 3;

    [SerializeField] private GameObject painelFinal;

    private bool lutaFinalizada = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SequenciaRound());
    }

    IEnumerator SequenciaRound()
    {
        GameManager.Instance.TravarControle();
        lutaAtiva = false;

        yield return StartCoroutine(MostrarTexto("ROUND" + roundAtual));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(MostrarTexto("LUTEEEEEM"));

        lutaAtiva = true;

        GameManager.Instance.LiberarControle();
    }

    public void OnKO(bool morreuPlayer)
    {
        if(!lutaAtiva || lutaFinalizada) return;

        if(morreuPlayer)
           enemyWins++;
        
        else
           playerWins++;

        StartCoroutine(SequenciaKO());
    }

    IEnumerator SequenciaKO()
    {
        lutaAtiva = false;
        GameManager.Instance.TravarControle();

        yield return StartCoroutine(MostrarTexto("K.O"));

        yield return new WaitForSeconds(1f);

        if(playerWins >= 2 || enemyWins >= 2)
        {
            lutaFinalizada = true;
            StartCoroutine(TelaFinal());
            yield break;
        }

        roundAtual++;

        ResetarLuta();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SequenciaRound());


    }

    IEnumerator TelaFinal()
    {
        string vencedor = playerWins > enemyWins ? "PLAYER VENCEU" : "INIMIGO VENCEU SEU FRACO";

        yield return StartCoroutine(MostrarTexto(vencedor));

        MostrarOpcoesFinais();
    }

    void MostrarOpcoesFinais()
    {
        painelFinal.SetActive(true);
    }

    public void JogarNovamente()
    {
        painelFinal.SetActive(false);

        playerWins = 0;
        enemyWins = 0;
        roundAtual = 1;
        lutaFinalizada = false;

        ResetarLuta();
        StartCoroutine(SequenciaRound());
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("menuprincipal");
    }

    void ResetarLuta()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        GameObject NewEnemy = GameObject.FindWithTag("Enemy");

        if(Player == null || NewEnemy == null)
        {
            Debug.LogError("Player ou Enemy não encontrado");
            return;
        }

        var pController = Player.GetComponent<CharacterController>();
        var eController = NewEnemy.GetComponent<CharacterController>();

        if (pController != null) pController.enabled = true;
        if (eController != null) eController.enabled = true;

        Player.transform.position = new Vector3(-7.524553f, 6.141839f, 2.323583f);
        NewEnemy.transform.position = new Vector3(9.035446f, 4.741839f, 2.593583f);
        
        Player.GetComponent<NewPlayMove>()?.ResetState();
        NewEnemy.GetComponent<EnemyIA>()?.ResetState();



        Player.GetComponent<HealthForAll>()?.ResetarVida();
        NewEnemy.GetComponent<HealthForAll>()?.ResetarVida();

        ResetarComponentes(Player);
        ResetarComponentes(NewEnemy);

    }

    void ResetarComponentes(GameObject obj)
    {
        var combat = obj.GetComponent<FightCombat>();
        if(combat != null) combat.enabled = true;

        var input = obj.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if(input != null) input.enabled = true;

    }

    IEnumerator MostrarTexto(string mensagem)
    {
        _texto.text = mensagem;

        // Fade In + Scale

        float tempo = 0f;
        float duracao = 0.5f;
        
        _texto.alpha = 0;
        _texto.transform.localScale = Vector3.one * 0.5f;

        while(tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = tempo / duracao;

            _texto.alpha = Mathf.Lerp(0,1, t);
            _texto.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, t);

            yield return null;
        }

        yield return new WaitForSeconds(tempoEntreTextos);

        // Fade Out
        tempo = 0f;

        while(tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = tempo / duracao;

            _texto.alpha = Mathf.Lerp(1,0,t);
            _texto.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, t);

            yield return null;
        }
    }
}
