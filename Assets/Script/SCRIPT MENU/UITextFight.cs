using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITextFight : MonoBehaviour
{
    public static UITextFight instance;

    [Header("Texto da Luta")]
    [SerializeField] private TextMeshProUGUI _texto;
    [SerializeField] private float tempoEntreTextos = 1.5f;

    [Header("Rounds")]
    [SerializeField] private int maxRounds = 3;

    private int roundAtual = 1;

    private int playerWins = 0;
    private int enemyWins = 0;

    private bool lutaAtiva = false;
    private bool lutaFinalizada = false;

    [Header("Tela Final")]
    [SerializeField] private GameObject painelFinal;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _tempoRound = 60f;

    private float _tempoAtual;
    private bool _timerRodando = false;

    [Header("Contador de Vitórias")]
    [SerializeField] private Image[] playerWinIcons;
    [SerializeField] private Image[] enemyWinIcons;


    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (painelFinal != null)
            painelFinal.SetActive(false);

        AtualizarHUDVitorias();

        StartCoroutine(SequenciaRound());
    }

    private void Update()
    {
        AtualizarTimer();
    }


    // =========================================================
    // INÍCIO DO ROUND
    // =========================================================

    private IEnumerator SequenciaRound()
    {
        GameManager.Instance.TravarControle();

        lutaAtiva = false;

        yield return StartCoroutine(
            MostrarTexto("ROUND " + roundAtual)
        );

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(
            MostrarTexto("LUTEEEEEM")
        );

        _tempoAtual = _tempoRound;
        _timerRodando = true;

        if (_timerText != null)
        {
            _timerText.text =
                Mathf.CeilToInt(_tempoAtual).ToString();
        }

        lutaAtiva = true;

        GameManager.Instance.LiberarControle();
    }


    // =========================================================
    // TIMER
    // =========================================================

    private void AtualizarTimer()
    {
        if (!_timerRodando || !lutaAtiva)
            return;

        _tempoAtual -= Time.deltaTime;

        if (_tempoAtual < 0)
            _tempoAtual = 0;

        if (_timerText != null)
        {
            _timerText.text =
                Mathf.CeilToInt(_tempoAtual).ToString();
        }

        if (_tempoAtual <= 0)
        {
            _timerRodando = false;

            VerificarVencedorPorTempo();
        }
    }


    // =========================================================
    // VITÓRIA POR TEMPO
    // =========================================================

    private void VerificarVencedorPorTempo()
    {
        if (!lutaAtiva || lutaFinalizada)
            return;

        lutaAtiva = false;

        CombatSide[] lutadores =
            FindObjectsByType<CombatSide>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        HealthForAll playerHealth = null;
        HealthForAll enemyHealth = null;

        foreach (CombatSide lutador in lutadores)
        {
            HealthForAll health =
                lutador.GetComponent<HealthForAll>();

            if (health == null)
                continue;

            if (lutador.CurrentSide ==
                CombatSide.Side.Player)
            {
                playerHealth = health;
            }
            else if (lutador.CurrentSide ==
                     CombatSide.Side.Enemy)
            {
                enemyHealth = health;
            }
        }

        if (playerHealth == null ||
            enemyHealth == null)
        {
            Debug.LogError(
                "UITextFight: Não foi possível encontrar a vida dos dois lutadores."
            );

            return;
        }

        if (playerHealth.CurrentHealth >
            enemyHealth.CurrentHealth)
        {
            playerWins++;

            Debug.Log(
                "PLAYER venceu o round por tempo!"
            );
        }
        else if (enemyHealth.CurrentHealth >
                 playerHealth.CurrentHealth)
        {
            enemyWins++;

            Debug.Log(
                "CPU/PLAYER 2 venceu o round por tempo!"
            );
        }
        else
        {
            Debug.Log(
                "Empate por tempo!"
            );
        }

        AtualizarHUDVitorias();

        StartCoroutine(SequenciaFimRound());
    }


    // =========================================================
    // KO
    // =========================================================

    public void OnKO(GameObject derrotado)
    {
        if (!lutaAtiva || lutaFinalizada)
            return;

        if (derrotado == null)
            return;

        CombatSide combatSide =
            derrotado.GetComponent<CombatSide>();

        if (combatSide == null)
        {
            Debug.LogError(
                $"{derrotado.name}: CombatSide não encontrado!"
            );

            return;
        }

        lutaAtiva = false;
        _timerRodando = false;


        // =====================================================
        // QUEM MORREU?
        // =====================================================

        if (combatSide.CurrentSide ==
            CombatSide.Side.Player)
        {
            enemyWins++;

            Debug.Log(
                $"{derrotado.name} era PLAYER e perdeu o round!"
            );
        }
        else if (combatSide.CurrentSide ==
                 CombatSide.Side.Enemy)
        {
            playerWins++;

            Debug.Log(
                $"{derrotado.name} era ENEMY e perdeu o round!"
            );
        }


        AtualizarHUDVitorias();

        StartCoroutine(SequenciaFimRound());
    }


    // =========================================================
    // FIM DO ROUND
    // =========================================================

    private IEnumerator SequenciaFimRound()
    {
        GameManager.Instance.TravarControle();

        lutaAtiva = false;
        _timerRodando = false;

        yield return StartCoroutine(
            MostrarTexto("K.O")
        );

        yield return new WaitForSeconds(1f);

        int vitoriasNecessarias =
            Mathf.CeilToInt(maxRounds / 2f);

        if (playerWins >= vitoriasNecessarias ||
            enemyWins >= vitoriasNecessarias)
        {
            lutaFinalizada = true;

            yield return StartCoroutine(
                TelaFinal()
            );

            yield break;
        }

        roundAtual++;

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(
            SequenciaRound()
        );
    }


    // =========================================================
    // TELA FINAL
    // =========================================================

    private IEnumerator TelaFinal()
    {
        string vencedor;

        if (playerWins > enemyWins)
        {
            vencedor = "PLAYER VENCEU";
        }
        else
        {
            vencedor = "INIMIGO VENCEU";
        }

        yield return StartCoroutine(
            MostrarTexto(vencedor)
        );

        MostrarOpcoesFinais();
    }

    private void MostrarOpcoesFinais()
    {
        if (painelFinal != null)
            painelFinal.SetActive(true);
    }


    // =========================================================
    // HUD DE VITÓRIAS
    // =========================================================

    private void AtualizarHUDVitorias()
    {
        if (playerWinIcons != null)
        {
            for (int i = 0;
                 i < playerWinIcons.Length;
                 i++)
            {
                if (playerWinIcons[i] != null)
                {
                    playerWinIcons[i]
                        .gameObject
                        .SetActive(i < playerWins);
                }
            }
        }

        if (enemyWinIcons != null)
        {
            for (int i = 0;
                 i < enemyWinIcons.Length;
                 i++)
            {
                if (enemyWinIcons[i] != null)
                {
                    enemyWinIcons[i]
                        .gameObject
                        .SetActive(i < enemyWins);
                }
            }
        }
    }


    // =========================================================
    // JOGAR NOVAMENTE
    // =========================================================

    public void JogarNovamente()
    {
        if (painelFinal != null)
            painelFinal.SetActive(false);

        playerWins = 0;
        enemyWins = 0;

        roundAtual = 1;

        lutaFinalizada = false;
        lutaAtiva = false;

        AtualizarHUDVitorias();

        StartCoroutine(
            SequenciaRound()
        );
    }


    // =========================================================
    // MENU PRINCIPAL
    // =========================================================

    public void MenuPrincipal()
    {
        GameManager.Instance.TravarControle();

        SceneManager.LoadScene(
            "menuprincipal"
        );
    }


    // =========================================================
    // TEXTO DA LUTA
    // =========================================================

    private IEnumerator MostrarTexto(string mensagem)
    {
        if (_texto == null)
            yield break;

        _texto.text = mensagem;

        float tempo = 0f;
        float duracao = 0.5f;

        _texto.alpha = 0f;

        _texto.transform.localScale =
            Vector3.one * 0.5f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float t =
                tempo / duracao;

            _texto.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    t
                );

            _texto.transform.localScale =
                Vector3.Lerp(
                    Vector3.one * 0.5f,
                    Vector3.one,
                    t
                );

            yield return null;
        }

        yield return new WaitForSeconds(
            tempoEntreTextos
        );

        tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float t =
                tempo / duracao;

            _texto.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    t
                );

            _texto.transform.localScale =
                Vector3.Lerp(
                    Vector3.one,
                    Vector3.one * 1.2f,
                    t
                );

            yield return null;
        }

        _texto.alpha = 0f;
    }
}
