using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    [Header("Painel")]
    [SerializeField] private GameObject loadingPanel;

    [Header("Sprite Carregando")]
    [SerializeField] private Image loadingImage;

    [Header("SimboloGirando")]
    [SerializeField] private Image loadingIcon;

    [Header("Config de Rotação")]
    [SerializeField] private float rotationDuration = 0.5f;

    [Header("Configuração")]
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private float minimumLoadingTime = 1.5f;

    private Coroutine loadingCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(loadingPanel != null)
           loadingPanel.SetActive(false);
    }

    public void StartLoading(string sceneName)
    {
        if (loadingCoroutine != null)
            return;

        loadingCoroutine = StartCoroutine(LoadSceneAsycn(sceneName));
    }

    private IEnumerator LoadSceneAsycn(string sceneName)
    {
        // Ativar painel
        loadingPanel.SetActive(true);

        PrepararLoading();

        float startTime = Time.time;

        // Começa o carregamento da cena 
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;


        // Mata qualquer animação anterior
        if (loadingImage != null)
        {
            loadingImage.DOKill();

            Color color = loadingImage.color;
            color.a = 1f;
            loadingImage.color = color;
        }

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        while(Time.time - startTime < minimumLoadingTime)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(PrepararFase());


        FinalizarLoading();

        loadingCoroutine = null;



    }

    private void PrepararLoading()
    {
        if (loadingImage != null)
        {
            loadingImage.DOKill();

            Color color = loadingImage.color;
            color.a = 1f;
            loadingImage.color = color;

            loadingImage
                .DOFade(0f, fadeDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

       if (loadingIcon != null)
        {
            loadingIcon.DOKill();

            loadingIcon.transform.localRotation =
                Quaternion.identity;

            loadingIcon.transform
                .DORotate(
                    new Vector3(0f, 0f, -360f),
                    rotationDuration,
                    RotateMode.FastBeyond360
                )
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        } 
    }

    private IEnumerator PrepararFase()
    {
        // Dá oportunidade para os sistemas da cena
        // terminarem de inicializar.

        yield return null;

        // Libera recursos que pertenciam à cena anterior
        AsyncOperation unloadUnused =
            Resources.UnloadUnusedAssets();

        yield return unloadUnused;

        // Dá mais um frame para os sistemas da fase
        // terminarem suas inicializações.

        yield return null;
        yield return new WaitForEndOfFrame();
    }

    private void FinalizarLoading()
    {
        if (loadingImage != null)
            loadingImage.DOKill();

        if (loadingIcon != null)
            loadingIcon.DOKill();

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

}
