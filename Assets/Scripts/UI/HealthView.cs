using UnityEngine;

public abstract class HealthView : MonoBehaviour
{
    [SerializeField] protected Health Health;

    protected void OnEnable()
    {
        Health.Restored += OnHealthChanged;
        Health.DamageTaken += OnHealthChanged;
        Health.Restored += OnHealthChanged;
        Health.Started += OnStarted;
    }

    protected void OnDisable()
    {
        Health.Restored -= OnHealthChanged;
        Health.DamageTaken -= OnHealthChanged;
        Health.Restored -= OnHealthChanged;
        Health.Started -= OnStarted;
    }

    protected void OnHealthChanged(float currentHealth)
    {
        ShowHealth(currentHealth);
    }

    protected void OnStarted()
    {
        InitHealth();
    }

    protected virtual void ShowHealth(float value) { }

    protected virtual void InitHealth() { }
}