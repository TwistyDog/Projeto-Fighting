using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FightCombat))]
public class FightCombatInput : MonoBehaviour
{
    private FightCombat _combat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _combat = GetComponent<FightCombat>();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Combat");
    }

    public void OnRightPuch(InputAction.CallbackContext context)
    {
        if (context.performed) _combat.RightPuch();
    }

    public void OnLeftPuch(InputAction.CallbackContext context)
    {
        if (context.performed) _combat.LeftPuch();
    }

    public void OnHighKick(InputAction.CallbackContext context)
    {
        if (context.performed) _combat.HighKick();
    }

    public void OnLowKick(InputAction.CallbackContext context)
    {
        if (context.performed) _combat.LowKick();
    }
   
}