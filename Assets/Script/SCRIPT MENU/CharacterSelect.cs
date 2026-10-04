using DG.Tweening;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    public static int SelectedPlayerCharacter;
    public static int SelectedEnemyCharacter;

    public static ControlType Player1Control;
    public static ControlType Player2Control;

    public static bool Player1IsCPU = false;
    public static bool Player2IsCPU = true;

    public static int SelectedCharacter;

    [SerializeField] private LoadingManager loadingManager;

    [Header("Cursor de Sele��o")]
    [SerializeField] private RectTransform selectionCursor;

    [Header("Posi��es dos Personagens")]
    [SerializeField] private RectTransform[] characterPosition;

    [Header("Configura��es")]
    [SerializeField] private float cursorMoveSpeed = 0.15f;

        private int currentCharacters = 0;

    private bool canSelect = false;

    private int selectionStep = 0;

    public enum ControlType
    {
        Player,
        CPU
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        currentCharacters = 0;
        selectionStep = 0;
        canSelect = false;

       if(selectionCursor == null ||
       characterPosition == null ||
       characterPosition.Length == 0)
        {
            return;
        }

        selectionCursor.gameObject.SetActive(true);

        selectionCursor.DOKill();

        selectionCursor.position =
            characterPosition[0].position;

        Invoke(nameof(EnableSelection), 0.5f);
    }

    private void OnDisable()
    {
        canSelect = false;

        CancelInvoke(nameof(EnableSelection));
    }

    private void EnableSelection()
    {
        canSelect = true;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        if(!canSelect)
           return;
        
        Vector2 input = context.ReadValue<Vector2>();

        if(input.sqrMagnitude < 0.25f)
           return;
        
        MoveSelection(input);
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if(!context.performed)
           return;

        if(!canSelect)
           return;
        
        ConfirmCharacter();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
      if (!context.performed)
            return;

        if (!canSelect)
            return;

        Debug.Log("Voltando do Character Select.");

        // Aqui você pode chamar o MainMenu posteriormente  
    }


    private void MoveSelection(Vector2 direction)
    {
        if (characterPosition == null || characterPosition.Length == 0)
            return;

        int bestIndex = -1;

        float bestScore = float.MaxValue;

        Vector3 currentPosition =
            characterPosition[currentCharacters].position;

        Vector2 normalizedDirection =
            direction.normalized;

        for (int i = 0; i < characterPosition.Length; i++)
        {
            if (i == currentCharacters)
                continue;

            Vector3 offset =
                characterPosition[i].position - currentPosition;

            Vector2 offset2D =
                new Vector2(offset.x, offset.y);

            float distance =
                offset2D.magnitude;

            if (distance <= 0.01f)
                continue;

            Vector2 candidateDirection =
                offset2D.normalized;

            // Verifica se o personagem está na direção
            // que estamos tentando navegar.
            float dot =
                Vector2.Dot(
                    normalizedDirection,
                    candidateDirection
                );

            // Quanto maior o dot, mais alinhado está
            // com a direção desejada.
            if (dot < 0.5f)
                continue;

            // Pontuação:
            // menor distância = melhor
            // maior alinhamento = melhor
            float score =
                distance / dot;

            if (score < bestScore)
            {
                bestScore = score;
                bestIndex = i;
            }
        }

        if (bestIndex == -1)
            return;

        currentCharacters = bestIndex;

        MoveCursor();
    }


    private void PreviousCharacter()
    {
        currentCharacters--;

        if (currentCharacters < 0)
            currentCharacters = characterPosition.Length -1;

        MoveCursor();
    }

    private void ConfirmCharacter()
    {
        // --------------------------------------
        // PRIMEIRA ESCOLHA = PLAYER
        // --------------------------------------

        if (selectionStep == 0)
        {
            SelectedPlayerCharacter =
                currentCharacters;


            // Mantém compatibilidade
            SelectedCharacter =
                currentCharacters;


            Debug.Log(
                "PLAYER 1 selecionou: " +
                SelectedPlayerCharacter +
                "| CPU: " +
                Player1IsCPU
            );


            selectionStep = 1;


            // Continua permitindo selecionar
            // o segundo personagem.
            canSelect = true;


            Debug.Log(
                "Agora selecione o personagem da CPU."
            );


            return;
        }


        // --------------------------------------
        // SEGUNDA ESCOLHA = CPU
        // --------------------------------------

        if (selectionStep == 1)
        {
            SelectedEnemyCharacter =
                currentCharacters;


            Debug.Log(
                "CPU selecionou: " +
                SelectedEnemyCharacter +
                "| CPU: " +
                Player2IsCPU
            );


            canSelect = false;


            // Agora sim começa o loading
            if (loadingManager != null)
            {
                loadingManager.StartLoading(
                    "Area de Rua"
                );
            }
            else
            {
                Debug.LogError(
                    "CharacterSelect: LoadingManager não configurado!"
                );
            }
        }


    }
    public void SelectCharacter(int id)
    {
        if (!canSelect)
            return;

        if (id < 0 || id >= characterPosition.Length)
            return;

        currentCharacters = id;

        MoveCursor();

        SelectedCharacter = id;

        ConfirmCharacter();
    }

    private void MoveCursor()
    {
        if (selectionCursor == null)
            return;

        if(currentCharacters <0 ||
            currentCharacters >= characterPosition.Length)
            return;

        selectionCursor.DOKill();

        selectionCursor
            .DOMove(
                characterPosition[currentCharacters].position,
                cursorMoveSpeed
            )
            .SetEase(Ease.OutQuad);
    }
}
