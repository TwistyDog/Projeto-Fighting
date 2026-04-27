using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UITextFight : MonoBehaviour
{
    public static UITextFight instance;

    public TextMeshProUGUI _texto;
    public float tempoEntreTextos = 1.5f;

    private int roundAtual = 1;
    private bool lutaAtiva = false;
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

    public void OnKO()
    {
        if(!lutaAtiva) return;

        StartCoroutine(SequenciaKO());
    }

    IEnumerator SequenciaKO()
    {
        lutaAtiva = false;
        GameManager.Instance.TravarControle();

        yield return StartCoroutine(MostrarTexto("K.O"));

        yield return new WaitForSeconds(1f);

        roundAtual++;

        ResetarLuta();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SequenciaRound());


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

        Player.SetActive(true);
        NewEnemy.SetActive(true);

        Player.transform.position = new Vector3(-7.524553f, 6.141839f, 2.323583f);
        NewEnemy.transform.position = new Vector3(9.035446f, 4.741839f, 2.593583f);

        ResetarComponentes(Player);
        ResetarComponentes(NewEnemy);


        Player.GetComponent<HealthForAll>()?.ResetarVida();
        NewEnemy.GetComponent<HealthForAll>()?.ResetarVida();


    }

    void ResetarComponentes(GameObject obj)
    {
        var controller = obj.GetComponent<FightCombat>();
        if(controller != null) controller.enabled = true;

        var combat = obj.GetComponent<FightCombat>();
        if(combat != null) controller.enabled = true;

        var input = obj.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if(input != null) input.enabled = true;

        var move = obj.GetComponent<NewPlayMove>();
        if(move != null)
        {
            move._playerVelocity = Vector3.zero;
        }

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
