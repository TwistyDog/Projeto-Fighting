using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public int buttonIndex;
    public MainMenu menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerEnter(PointerEventData eventData)
    {
        menu.SelectedButton(buttonIndex);
    }
}
