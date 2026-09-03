using DG.Tweening;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        currentCharacters = 0;
        canSelect = false;

        if (selectionCursor == null || characterPosition.Length == 0)
            return;

        selectionCursor.gameObject.SetActive(true);

        selectionCursor.DOKill();

        selectionCursor.position =
            characterPosition[0].position;

        Invoke(nameof(EnableSelection), 0.5f);
    }

    private void EnableSelection()
    {
        canSelect = true;
    }

    private void Update()
    {
        if (!canSelect)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCharacter();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmCharacter();
        }
    }

    private void NextCharacter()
    {
        currentCharacters++;

        if (currentCharacters >= characterPosition.Length)
            currentCharacters = 0;

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
        SelectedCharacter = currentCharacters;

        Debug.Log("Personagem Selecionado" + SelectedCharacter);

        canSelect = false;

        if(loadingManager != null)
        {
            loadingManager.StartLoading("Area de Rua");
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
