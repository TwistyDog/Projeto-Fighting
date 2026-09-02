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
    [SerializeField] GameObject _gameModePanel;
    [SerializeField] GameObject _characterSelectPanel;

    [SerializeField] private Button[] menuButtons;

    [Header("Botoes de Modo")]
    [SerializeField] private Button[] _gameModeButton;

    [Header("Visual Selecao")]
    [SerializeField] Image _backgroundImage;

    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite OptionsSprite;
    [SerializeField] private Sprite quitSprite;

    [Header("Visual Game Mode")]
    [SerializeField] private Image _gameModeBackGround;

    [SerializeField] private Sprite _arcadeSprite;
    [SerializeField] private Sprite _onlineSprite;
    [SerializeField] private Sprite _treinamentoSprite;
    [SerializeField] private Sprite _voltarSprite;



    private int currentIndex = 0;

    private int gameModeIndex = 0;

    private GameMode _selectedMode;

    private void Start()
    {
        AnimateMenuButtons();
        //UptadeSelection();

    }

    public void SelectedButton(int index)
    {
        currentIndex = index;
        UptadeSelection();
    }

    private void Update()
    {
        if (_mainMenuPainel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex++;

                if (currentIndex >= menuButtons.Length)
                    currentIndex = 0;

                UptadeSelection();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex--;

                if (currentIndex < 0)
                    currentIndex = menuButtons.Length - 1;

                UptadeSelection();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteSelected();
                return; // <<< IMPORTANTE
            }
        }


        // ==========================================
        // SELEÇÃO DE MODO
        // ==========================================

        if (_gameModePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gameModeIndex++;

                if (gameModeIndex >= _gameModeButton.Length)
                    gameModeIndex = 0;

                UptadeGameModeSelection();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gameModeIndex--;

                if (gameModeIndex < 0)
                    gameModeIndex = _gameModeButton.Length - 1;

                UptadeGameModeSelection();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelecionarModo(gameModeIndex);
                return;
            }
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

    private void ChangeBackGroundGameMode()
    {
        Sprite nextSprite = null;

        switch (gameModeIndex)
        {
            case 0:
                nextSprite = _arcadeSprite;
                break;

            case 1:
                nextSprite = _onlineSprite;
                break;

            case 2:
                nextSprite = _treinamentoSprite;
                break;

            case 3:
                nextSprite = _voltarSprite;
                break;
        }

        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() =>
        {
            _gameModeBackGround.sprite = nextSprite;

            _gameModeBackGround.rectTransform.localScale =
               Vector3.one * 1.15f;
        });

        seq.Append(
            _gameModeBackGround
            .DOFade(1f, 0.25f));


        _gameModeBackGround.rectTransform
            .DOScale(1f, 0.4f)
            .SetEase(Ease.OutQuad);
    }



    public void NovoJogo()
    {
        _mainMenuPainel.SetActive(false);
        _gameModePanel.SetActive(true);
        _characterSelectPanel.SetActive(false);

        gameModeIndex = 0;
        UptadeGameModeSelection();
    }

    public void VoltarMenu()
    {
        _characterSelectPanel.SetActive(false);
        _gameModePanel.SetActive(false);
        _mainMenuPainel.SetActive(true);
    }

    public void VoltarParaGameMode()
    {
        _characterSelectPanel.SetActive(false);
        _gameModePanel.SetActive(true);
        _mainMenuPainel .SetActive(false);

        gameModeIndex = (int)_selectedMode;

        UptadeGameModeSelection();
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
        //Desativa todos os botoes no inicio
        foreach (Button button in menuButtons)
        {
            button.interactable = false;

            CanvasGroup canvasGroup = button.gameObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = button.gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
        }

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < menuButtons.Length; i++)
        {
            Button button = menuButtons[i];

            RectTransform rect = button.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();

            Vector2 originalPos = rect.anchoredPosition;

            //Começa fora da tela 
            rect.anchoredPosition =
                new Vector2(originalPos.x - 800f, originalPos.y);

            // Movimento do Botão
            sequence.Insert(
                i * 0.1f,
                rect.DOAnchorPos(originalPos, 0.8f)
                    .SetEase(Ease.OutBack)
                    );
        }

        //Tempo necessário para todos os botões terminarem de entrar

        float tempoEntrada =
            0.8f + ((menuButtons.Length - 1) * 0.1f);

        //Depois que todos chegaram, começa o Fade

        sequence.InsertCallback(tempoEntrada, () =>
        {
            Sequence fadeSequence = DOTween.Sequence();

            for (int i = 0; i < menuButtons.Length; i++)
            {
                CanvasGroup canvasGroup =
                    menuButtons[i].GetComponent<CanvasGroup>();

                fadeSequence.Insert(
                    i * 0.1f,
                    canvasGroup.DOFade(1f, 0.35f)
                    .SetEase(Ease.OutQuad));
            }

            //Depois do Fade, libera os botões
            fadeSequence.OnComplete(() =>
            {
                foreach (Button button in menuButtons)
                {
                    button.interactable = true;
                }

                UptadeSelection();
            });
        });

    }

    private void UptadeGameModeSelection()
    {
        for(int i = 0; i < _gameModeButton.Length; i++)
        {
            if(i == gameModeIndex)
            {
                _gameModeButton[i].transform
                    .DOScale(1.15f, 0.2f)
                    .SetEase(Ease.OutQuad);
            }
            else
            {
                _gameModeButton[i].transform
                    .DOScale(1f, 0.2f)
                    .SetEase(Ease.OutQuad); 
            }
        }

        ChangeBackGroundGameMode();
    }

    public void SelecionarModo(int modo)
    {
        _selectedMode = (GameMode)modo;

        Debug.Log("Opção selecionada: " + _selectedMode);

        switch (_selectedMode)
        {
            case GameMode.Arcade:
                AbrirCharacterSelect();
                break;

            case GameMode.Online:
                AbrirCharacterSelect();
                break;

            case GameMode.Treinamento:
                AbrirCharacterSelect();
                break;

            case GameMode.Voltar:
                VoltarMenu();
                break;
        }
    }

    public void AbrirCharacterSelect()
    {
        _mainMenuPainel.SetActive(false);
        _gameModePanel.SetActive(false);
        _characterSelectPanel.SetActive(true);

        Debug.Log(
        "Abrindo Character Select. Modo: " +
        _selectedMode
    );
    }

    public enum GameMode
    {
        Arcade,
        Online,
        Treinamento,
        Voltar
    }

}
