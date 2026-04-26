using System.Collections;
using TMPro;
using UnityEngine;

public class UITextFight : MonoBehaviour
{
    public TextMeshProUGUI _texto;
    public float tempoEntreTextos = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SequenciaRound());
    }

    IEnumerator SequenciaRound()
    {
        GameManager.Instance.TravarControle();
        
        yield return StartCoroutine(MostrarTexto("ROUND 1"));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(MostrarTexto("LUTEEEEEM"));

        GameManager.Instance.LiberarControle();
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
