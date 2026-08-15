using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LogicGameControl : MonoBehaviour
{
    [Header("Painel de Pause")]
    [SerializeField] private GameObject _pausePanel;

    [Header("Anima��o")]
    [SerializeField] private float _animationDuration = 0.25f;

    [SerializeField] private Vector3 startScale = new Vector3(0.85f, 0.85f, 1f);

    [Header("Canvas do Pause")]
    [SerializeField] private CanvasGroup _pauseCanvasGroup;

    [Header("Sele��o")]
    [SerializeField] private GameObject _firstSelectedButton;
    [SerializeField] private GameObject[] _pauseButtons;

    [Header("Destaque dos Botões")]
    [SerializeField] private float _normalButtonScale = 1f;
    [SerializeField] private float _selectedScale = 1.08f;
    [SerializeField] private float _scaleSpeed = 10f;

    private RectTransform _pauseRect;

    private bool isPaused = false;
    private bool isAnimated = false;

    private Coroutine _currentAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if(_pausePanel == null)
        {
            Debug.LogError(
                "LogicGameControl: Pause Panel não foi confifurado!"
            );

            return;
        }

        _pauseRect = _pausePanel.GetComponent<RectTransform>();

        if(_pauseCanvasGroup == null)
        {
            _pauseCanvasGroup = 
                   _pausePanel.AddComponent<CanvasGroup>();
        }

        _pausePanel.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        UpdateHiglightButton();
    }

    private void UpdateHiglightButton()
    {
        if(_pauseButtons == null || _pauseButtons.Length == 0)
           return;
        
        EventSystem eventSystem = EventSystem.current;

        if(eventSystem == null)
           return;
        
        GameObject selectedObject = eventSystem.currentSelectedGameObject;

        foreach (GameObject button in _pauseButtons)
        {
            if(button == null)
              continue;
            
            RectTransform rect = button.GetComponent<RectTransform>();

            if(rect == null)
              continue;
            
            float targetScale = 
                  button == selectedObject
                  ? _selectedScale
                  : _normalButtonScale;
            
            Vector3 target =
               Vector3.one * targetScale;
            
            rect.localScale = Vector3.Lerp(
                rect.localScale,
                target,
                _scaleSpeed * Time.unscaledDeltaTime
            );
        }
    }

    public void TogglePause()
    {
        if (isAnimated)
            return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (_pausePanel == null)
            return;

        isPaused = true;

        _pausePanel.SetActive(true);

        _pauseRect.localScale = startScale;
        _pauseCanvasGroup.alpha = 0f;

        Time.timeScale = 0f;

        if (_currentAnimation != null)
            StopCoroutine(AnimatedPauseIn());

        _currentAnimation =
            StartCoroutine(AnimatedPauseIn());

        SelectedFirstButton();
    }

    private void ResumeGame()
    {
        if (_pausePanel == null)
            return;

        isPaused = false;

        if(_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _currentAnimation =
            StartCoroutine(AnimatedPauseOut());
    }

    private IEnumerator AnimatedPauseIn()
    {
        isAnimated = true;

        float elapsed = 0f;

        while (elapsed < _animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(elapsed / _animationDuration);

            progress =
                Mathf.SmoothStep(0f, 1f, progress);

            _pauseCanvasGroup.alpha = progress;

            _pauseRect.localScale =
                Vector3.Lerp(
                    startScale, Vector3.one,
                    progress
                    );

            yield return null;

        }

        _pauseCanvasGroup.alpha = 1f;
        _pauseRect.localScale = Vector3.one;

        isAnimated = false;
        _currentAnimation = null;
    }

    private IEnumerator AnimatedPauseOut()
    {
        isAnimated = true;

        float elapsed = 0f;

        Vector3 initialScale =
            _pauseRect.localScale;

        while(elapsed < _animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = 
                Mathf.Clamp01(elapsed / _animationDuration);

            progress =
                Mathf.SmoothStep(0f, 1f, progress);

            _pauseCanvasGroup.alpha =
                Mathf.Lerp(
                    1,
                    0f,
                    progress
                    );

            _pauseRect.localScale =
                Vector3.Lerp(
                    initialScale,
                    startScale,
                    progress
                    );

            yield return null;
        }

        _pauseCanvasGroup.alpha = 0f;
        _pauseRect.localScale = startScale;

        _pausePanel.SetActive(false);

        Time.timeScale = 1f;

        isAnimated = false;
        _currentAnimation = null;
    }

    private void SelectedFirstButton()
    {
        if(_firstSelectedButton == null)
           return;
        
        EventSystem eventSystem =
             EventSystem.current;
        
        if(eventSystem == null)
           return;
        
        eventSystem.SetSelectedGameObject(null);

        eventSystem.SetSelectedGameObject(
            _firstSelectedButton
        );

    }

    public void ForceResume()
    {
        if(!isPaused)
           return;
        
        if(_currentAnimation != null)
           StopCoroutine(_currentAnimation);
        
        _pausePanel.SetActive(false);

        _pauseCanvasGroup.alpha = 0;
        _pauseRect.localScale = startScale;

        isPaused = false;
        isAnimated = false;

        Time.timeScale = 1f;

    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    public void Continuar()
    {
        if (!isPaused)
            return;

        ResumeGame();
    }

    public void ReiniciarLuta()
    {
        if (isAnimated)
            return;

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        if (isAnimated)
            return;

        Time.timeScale = 1f;

        SceneManager.LoadScene("menuprincipal");
    }
}
