using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _minHealth = 0f;

    private float _health;
    private WaitForSeconds _delayBeforeDie;

    public event Action AfterDied;
    public event Action Died;
    public event Action<float> DamageTaken;
    public event Action<float> Restored;
    public event Action Started;

    public float MaxHealth => _maxHealth;
    public float MinHealth => _minHealth;
    public float CurrentHealth => _health;

    private void Awake()
    {
        _health = _maxHealth;
        Started?.Invoke();

        float dieDelay = 0.6f;
        _delayBeforeDie = new WaitForSeconds(dieDelay);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
            StartCoroutine(HandleDie());

        DamageTaken?.Invoke(_health);
    }

    public void Restore(float healthPoints)
    {
        if (_health + healthPoints > _maxHealth)
            _health = _maxHealth;
        else
            _health += healthPoints;

        Restored?.Invoke(_health);
    }

    private IEnumerator HandleDie()
    {
        Died?.Invoke();

        yield return _delayBeforeDie;

        AfterDied?.Invoke();
    }
}