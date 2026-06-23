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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingBar;

    [Header("SimboloChinês")]
    [SerializeField] RectTransform chineseSymbols;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartLoading(string sceneName)
    {
        loadingPanel.SetActive(true);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);
        
        operation.allowSceneActivation = false;

        float fakeProgess = 0f;

        while (fakeProgess < 1f)
        {
            fakeProgess += Time.deltaTime * 0.5f;

            loadingBar.fillAmount = fakeProgess;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        operation.allowSceneActivation = true;
    }
}
