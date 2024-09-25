using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _health;
    private WaitForSeconds _delayBeforeDie;

    public event Action AfterDied;
    public event Action Died;
    public event Action DamageTaken;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _health;

    private void Awake()
    {
        _health = _maxHealth;

        float _dieDelay = 0.6f;
        _delayBeforeDie = new WaitForSeconds(_dieDelay);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
            StartCoroutine(HandleDie());

        DamageTaken?.Invoke();
    }

    public void UseFirstAidKit(float healthPoints)
    {
        if (_health + healthPoints > _maxHealth)
            _health = _maxHealth;
        else
            _health += healthPoints;
    }

    private IEnumerator HandleDie()
    {
        Died?.Invoke();

        yield return _delayBeforeDie;

        AfterDied?.Invoke();
    }
}
