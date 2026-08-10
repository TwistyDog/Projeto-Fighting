using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthForAll : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Slider _healthSlider;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _currentHealth = _maxHealth;

        UpdateHealthUI();
    }

    public void SetHealthSlider(Slider slider)
    {
        _healthSlider = slider;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if(_healthSlider == null)
        return;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} RECEBEU DANO: {damage}");
        
        _currentHealth = Mathf.Clamp(_currentHealth - damage,0, _maxHealth);

        UpdateHealthUI();

        if(_currentHealth <= 0)
            Die();
    }

    public void ResetarVida()
    {
        _currentHealth = _maxHealth;

        UpdateHealthUI();
    }

    void Die()
    {
        if(UITextFight.instance != null)
        {
            bool isPlayer = CompareTag("Player");
            UITextFight.instance.OnKO(isPlayer);
        }

        Debug.Log($"{gameObject.name} foi derrotado");

        var controller = GetComponent<CharacterController>();
        if(controller != null ) controller.enabled = false;

        var combat = GetComponent<FightCombat>();
        if (combat != null) combat.enabled = false;

        var input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (input != null) input.enabled = false;

        
        // aqui depois: animação KO, freeze, etc
    }

}
