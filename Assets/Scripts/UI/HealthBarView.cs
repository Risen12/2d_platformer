using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarView : HealthView
{
    protected Slider Slider;

    protected void Awake()
    {
        Slider = GetComponent<Slider>();
        Slider.minValue = Health.MinHealth;
        Slider.maxValue = Health.MaxHealth;
    }

    protected override void ShowHealth(float value)
    {
        Slider.value = Mathf.Clamp(value, Health.MinHealth, Health.MaxHealth);
    }

    protected override void InitHealth()
    {
        Slider.value = Health.MaxHealth;
    }
}