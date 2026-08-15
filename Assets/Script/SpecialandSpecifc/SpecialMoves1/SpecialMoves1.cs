using UnityEngine;
using UnityEngine.InputSystem;

public class SpecialMoves1 : NewPlayMove
{
    [Header("Special Attack")]
    [SerializeField] private GameObject projectillePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float specialCoolDown = 1.5f;

    [Header("Carga do Especial")]
    [SerializeField] private float _chargeTime = 1.0f;

    [Header("Direcao")]
    [SerializeField] private float _backwardInputThreshold = -0.5f;
    [SerializeField] private float _forwardInputThreshold = 0.5f;

    private float lastSpecialTime;

    private float backWardTimer;

    private bool chargingSpecial;
    private bool specialReady;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void PlayerStart1()
    {

    }

    protected virtual void Awake()
    {

    }

    protected override void UpdateSpecialLogic()
    {
        if (!GameManager.Instance.podeControlar)
            return;

        // ==========================================
        // 1 - SEGURANDO PARA TRÁS
        // ==========================================

        if (!chargingSpecial && !specialReady)
        {
            if (_moveInput.x <= _backwardInputThreshold)
            {
                backWardTimer += Time.deltaTime;

                if (backWardTimer >= _chargeTime)
                {
                    chargingSpecial = true;

                    Debug.Log(
                        "ESPECIAL CARREGADO! AGORA APERTE PARA FRENTE."
                    );
                }
            }
            else
            {
                backWardTimer = 0f;
            }
        }

        // ==========================================
        // 2 - DEPOIS DE CARREGAR, IR PARA FRENTE
        // ==========================================

        if (chargingSpecial)
        {
            if (_moveInput.x >= _forwardInputThreshold)
            {
                specialReady = true;
                chargingSpecial = false;

                Debug.Log(
                    "ESPECIAL PRONTO! APERTE E."
                );
            }
        }
    }




    public void OnSpecial(InputAction.CallbackContext context)
    {

        if(!GameManager.Instance.podeControlar)
          return;


        if(!context.performed) return;

        if (!specialReady)
        {
            Debug.Log("Especial ainda não esta pronto!");
            return;
        }

        if(Time.time < lastSpecialTime + specialCoolDown)
           return;

        
        ShootProjectille();
        lastSpecialTime = Time.time;

        ResetSpecial();
    }

    void ShootProjectille()
    {

        if(!GameManager.Instance.podeControlar)
           return;

           if(projectillePrefab == null)
        {
            Debug.LogWarning("SpecialMoves1: Projetil Prefab não configurado!");
            return;
        }

        if(firePoint == null)
        {
            Debug.LogWarning("SpecialMoves1: Fire Point não configurado!");
            return;
        }

        Vector3 spawnPos = 
              firePoint.position + transform.right * 0.7f;
        
        GameObject proj =
            Instantiate(
                projectillePrefab,
                spawnPos,
                Quaternion.identity
            );
        
        Vector3 dir = transform.right;

        Projectille projectille =
            proj.GetComponent<Projectille>();
        
        if(projectille != null)
        {
            projectille.SetDirection(dir);
        }

        Debug.Log("Projetil Disparado");
    }

    private void ResetSpecial()
    {
        backWardTimer = 0f;

        chargingSpecial = false;
        specialReady = false;
    }
}
