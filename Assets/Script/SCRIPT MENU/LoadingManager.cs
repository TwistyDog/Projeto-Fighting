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

        // Mata qualquer animação anterior
        if (loadingImage != null)
        {
            loadingImage.DOKill();

            Color color = loadingImage.color;
            color.a = 1f;
            loadingImage.color = color;
        }

        // Começa animação de "CARREGAMENTO" 
        if(loadingImage != null)
        {
            loadingImage
                .DOFade(0f, fadeDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        if(loadingIcon != null)
        {
            loadingIcon.DOKill();

            loadingIcon.transform.localRotation = Quaternion.identity;

            loadingIcon.transform
                .DORotate(
                new Vector3(0f, 0f, -360f),
                rotationDuration,
                RotateMode.FastBeyond360
                )
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        // Começa o carregamento da cena 
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        float startTime = Time.time;

        while (!operation.isDone)
        {
            // Impede que a cena seja ativada antes do tempo mínimo
            if(operation.progress >= 0.9f &&
                Time.time - startTime > minimumLoadingTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
