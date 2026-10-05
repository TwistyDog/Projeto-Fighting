using DG.Tweening;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudioIntro : MonoBehaviour
{
    [Header("Logo do Estudio")]
    [SerializeField] private Image _logo;

    [Header("Configuração da Animação")]
    [SerializeField] private float duracaoEntrada = 2f;
    [SerializeField] private float tempoNaTela = 1.5f;
    [SerializeField] private float duracaoSaida = 0.8f;

    [Header("Cena do Menu")]
    [SerializeField] private string nomeCenaMenu = "menuprincipal";



    private void Start()
    {
        StartCoroutine(AnimacaoStudio());
    }

    private IEnumerator AnimacaoStudio()
    {
        if (_logo == null)
        {
            Debug.LogError(
                "StudioIntro: Logo do estúdio não foi configurado");

            IrParaMenu();
            yield break;
        }

        _logo.gameObject.SetActive(true);

        _logo.DOKill();

        Color cor = _logo.color;

        cor.a = 0f;
        _logo.color = cor;


        yield return _logo
            .DOFade(1f, duracaoEntrada)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();


        yield return new WaitForSeconds(
            tempoNaTela);

        yield return _logo
            .DOFade(0f, duracaoSaida)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();

        IrParaMenu();
    }

    private void IrParaMenu()
    {
        SceneManager.LoadScene(
            nomeCenaMenu);
    }
}
