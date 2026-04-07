using UnityEngine;
using UnityEngine.UI;

public class HealthForAll : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Slider _healthSlider;

    private int _currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _currentHealth = _maxHealth;

        if(_healthSlider != null)
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage,0, _maxHealth);

        if(_healthSlider != null)
           _healthSlider.value = _currentHealth;

        if(_currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} foi derrotado");

        var controller = GetComponent<CharacterController>();
        if(controller != null ) controller.enabled = false;

        var combat = GetComponent<FightCombat>();
        if (combat != null) combat.enabled = false;

        var input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (input != null) input.enabled = false;

        gameObject.SetActive(false);
        // aqui depois: animação KO, freeze, etc
    }
}
