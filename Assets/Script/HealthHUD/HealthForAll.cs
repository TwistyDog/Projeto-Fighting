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

    private void Awake()
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
        if (_healthSlider == null)
            return;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(
            $"{gameObject.name} RECEBEU DANO: {damage}"
        );

        _currentHealth = Mathf.Clamp(
            _currentHealth - damage,
            0,
            _maxHealth
        );

        UpdateHealthUI();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetarVida()
    {
        _currentHealth = _maxHealth;

        UpdateHealthUI();
    }

    private void Die()
    {
        Debug.Log(
            $"{gameObject.name} foi derrotado."
        );

        // Avisa o sistema da luta qual personagem morreu.
        if (UITextFight.instance != null)
        {
            UITextFight.instance.OnKO(gameObject);
        }

        CharacterController controller =
            GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        FightCombat combat =
            GetComponent<FightCombat>();

        if (combat != null)
            combat.enabled = false;

        UnityEngine.InputSystem.PlayerInput input =
            GetComponent<UnityEngine.InputSystem.PlayerInput>();

        if (input != null)
            input.enabled = false;
    }
}
