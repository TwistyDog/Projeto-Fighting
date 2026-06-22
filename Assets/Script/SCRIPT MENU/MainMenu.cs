using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nomeCenaJogo = "Area de Rua";
    public string nomeOpcoes = "OptionScene";

    [Header("Paineis")]
    [SerializeField] GameObject _mainMenuPainel;
    [SerializeField] GameObject _characterSelectPanel;

    [SerializeField] private Button[] menuButtons;

    [Header("Visual Selecao")]
    [SerializeField] Image _backgroundImage;

    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite OptionsSprite;
    [SerializeField] private Sprite quitSprite;


    private int currentIndex = 0;

    private void Start()
    {
        AnimateMenuButtons();
        UptadeSelection();
        
    }

    public void SelectedButton(int index)
    {
        currentIndex = index;
        UptadeSelection();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;

            if(currentIndex >= menuButtons.Length)
                currentIndex = 0;

            UptadeSelection();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;

            if(currentIndex < 0)
                currentIndex = menuButtons.Length - 1;
            
            UptadeSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteSelected();
        }
    }

    void UptadeSelection()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            Image img = menuButtons[i].GetComponent<Image>();

            if (i == currentIndex)
            {
                menuButtons[i].transform
                    .DOScale(1.15f, 0.2f);
            }
            else
            {
                menuButtons[i].transform
                    .DOScale(1f, 0.2f);
            }
        }

        ChangeBackground();
    }

    void ChangeBackground()
    {
        Sprite nextSprite = null;

        switch (currentIndex)
        {
            case 0:
                nextSprite = playSprite;
                break;
            case 1:
                nextSprite = OptionsSprite;
                break;
            case 2:
                nextSprite = quitSprite;
                break;
        }

        Sequence seq = DOTween.Sequence();

        seq.Append(_backgroundImage.DOFade(0f, 0.15f));

        seq.AppendCallback(() =>
        {
            _backgroundImage.sprite = nextSprite;

            _backgroundImage.rectTransform.localScale =
            Vector3.one * 1.15f;
        });

        seq.Append(_backgroundImage.DOFade(1f, 0.25f));

        _backgroundImage.rectTransform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutQuad);
    }

    void ExecuteSelected()
    {
        switch (currentIndex)
        {
            case 0:
                NovoJogo();
                break;
            case 1:
                Opcoes();
                break;
            case 2:
                Sair();
                break;
        }
    }



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

    void AnimateMenuButtons()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            RectTransform rect =
                menuButtons[i].GetComponent<RectTransform>();

            Vector2 originalPos = rect.anchoredPosition;

            rect.anchoredPosition =
                new Vector2(originalPos.x -800f, originalPos.y);

            rect.DOAnchorPos(
            originalPos,
            0.8f)
            .SetEase(Ease.OutBack)
            .SetDelay(i * 0.1f);
        }
    }

}
