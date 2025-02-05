using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private AttackPoint _attackPoint;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackDamage;
    [SerializeField] private LayerMask _playerLayerMask;

    private Health _health;
    private EnemyMover _enemyMover;
    private float _stopDelay;
    private float _delayBetweenAttacks;
    private WaitForSeconds _attackPhaseDuration;
    private bool _canAttack;
    private Coroutine _attackPhaseCoroutine;

    public event Action Attacking;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _enemyMover = GetComponent<EnemyMover>();

        _stopDelay = 0.5f;
        _delayBetweenAttacks = 1.5f;

        _attackPhaseDuration = new WaitForSeconds(_delayBetweenAttacks);
        _canAttack = false;
    }

    private void OnEnable()
    {
        _attackPoint.AttackStateChanged += OnAttackStateChanged;
        _health.AfterDied += OnAfterDied;
    }

    private void OnDisable()
    {
        _attackPoint.AttackStateChanged -= OnAttackStateChanged;
        _health.AfterDied -= OnAfterDied;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
        {
            _enemyMover.Stop(_stopDelay);
            _health.TakeDamage(bullet.DamagePerShot);
        }
    }

    private void OnAttackStateChanged(bool state)
    {
        if (state)
        {
            _canAttack = true;
            _attackPhaseCoroutine = StartCoroutine(StartAttackPhase());
        }
        else
        {
            _canAttack = false;

            if (_attackPhaseCoroutine != null)
                StopCoroutine(_attackPhaseCoroutine);
        }
    }

    private IEnumerator StartAttackPhase()
    {
        while (_canAttack)
        {
            Attacking?.Invoke();
            _enemyMover.Stop(_delayBetweenAttacks);

            Collider2D collider = Physics2D.OverlapCircle(_attackPoint.transform.position, _attackRadius, _playerLayerMask);

            if (collider.TryGetComponent(out Health health))
            {
                health.TakeDamage(_attackDamage);
            }

            yield return _attackPhaseDuration;
        }
    }

    private void OnAfterDied()
    {
        _enemyMover.Stop(_stopDelay);
        gameObject.SetActive(false);
    }
}