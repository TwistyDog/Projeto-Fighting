using UnityEngine;
using UnityEngine.InputSystem;

public class SpecialMoves1 : NewPlayMove
{
    [Header("Special Attack")]
    [SerializeField] private GameObject projectillePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float specialCoolDown = 1.5f;

    private float lastSpecialTime;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void PlayerStart1()
    {

    }

    protected virtual void Awake()
    {

    } 


    public void OnSpecial(InputAction.CallbackContext context)
    {
        if(!context.performed) return;

        if(Time.time < lastSpecialTime + specialCoolDown)
           return;

        
        ShootProjectille();
        lastSpecialTime = Time.time;
    }

    void ShootProjectille()
    {
        if (projectillePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectillePrefab, firePoint.position, Quaternion.identity);

        Vector3 dir = transform.right;

        proj.GetComponent<Projectille>()?.SetDirection(dir);
    }


    
   
}
