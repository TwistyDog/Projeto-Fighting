using DG.Tweening;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    public static int SelectedCharacter;

    [SerializeField] private LoadingManager loadingManager;

    [Header("Cursor de Seleção")]
    [SerializeField] private RectTransform selectionCursor;

    [Header("Posições dos Personagens")]
    [SerializeField] private RectTransform[] characterPosition;

    [Header("Configurações")]
    [SerializeField] private float cursorMoveSpeed = 0.15f;

        private int currentCharacters = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        currentCharacters = 0;

        if (selectionCursor == null || characterPosition.Length == 0)
            return;

        selectionCursor.gameObject.SetActive(true);

        selectionCursor.anchoredPosition =
            characterPosition[currentCharacters].anchoredPosition;
    }
    public void SelectCharacter(int id)
    {
        currentCharacters = id;
        SelectedCharacter = id;

        MoveCursor();

        loadingManager.StartLoading("Area de Rua");
    }

    private void MoveCursor()
    {
        if (selectionCursor == null)
            return;

        if(currentCharacters <0 ||
            currentCharacters >= characterPosition.Length)
            return;

        selectionCursor
            .DOAnchorPos(
                characterPosition[currentCharacters].anchoredPosition,
                cursorMoveSpeed
            )
            .SetEase(Ease.OutQuad);
    }
}
