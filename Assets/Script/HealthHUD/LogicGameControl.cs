using System.Collections;
using UnityEngine;

public class LogicGameControl : MonoBehaviour
{
    [Header("Painel de Pause")]
    [SerializeField] private GameObject _pausePanel;

    [Header("Animação")]
    [SerializeField] private float _animationDuration = 0.25f;

    [SerializeField] private Vector3 startScale = new Vector3(0.85f, 0.85f, 1f);

    [Header("Canvas do Pause")]
    [SerializeField] private CanvasGroup _pauseCanvasGroup;

    [Header("Seleção")]
    [SerializeField] private GameObject _firstSelectedButton;

    private RectTransform _pauseRect;

    private bool isPaused = false;
    private bool isAnimated = false;

    private Coroutine _currentAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        
    }

    private void Update()
    {
        
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

    }

    public void ForceResume()
    {

    }
}
