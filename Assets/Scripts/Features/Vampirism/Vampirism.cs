using System;
using System.Collections;
using UnityEngine;

public class Vampirism : MonoBehaviour
{
    [SerializeField] private float _delayDuration;
    [SerializeField] private float _takingHealthPerTime;

    private WaitForSeconds _delay;
    private Coroutine _vampireCoroutine;

    public event Action<float> HealthTaken;

    private void Awake()
    {
        _delay = new WaitForSeconds(_delayDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            TakeHealthFromTarget(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (_vampireCoroutine != null)
                StopCoroutine(_vampireCoroutine);
        }
    }

    private void TakeHealthFromTarget(Enemy enemy)
    {
        if (enemy.TryGetComponent(out Health health))
        {
            if (_vampireCoroutine != null)
                StopCoroutine(_vampireCoroutine);

            _vampireCoroutine = StartCoroutine(TakeHealth(health));
        }
    }

    private IEnumerator TakeHealth(Health enemyHealth)
    {
        while (gameObject.activeSelf)
        {
            enemyHealth.TakeDamage(_takingHealthPerTime);
            HealthTaken?.Invoke(_takingHealthPerTime);

            yield return _delay;
        }
    }
}