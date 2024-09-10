using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private AttackPoint _attackPoint;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackDamage;
    [SerializeField] private LayerMask _playerLayerMask;

    private float _health;
    private float _stopDelay;
    private float _delayBetweenAttacks;
    private EnemyMover _enemyMover;
    private WaitForSeconds _dieDelay;
    private WaitForSeconds _attackPhaseDuration;
    private bool _canAttack;
    private Coroutine _attackPhaseCoroutine;

    public event Action Attacking;
    public event Action DamageTaken;
    public event Action Died;

    private void Awake()
    {
        _health = _startHealth;
        _enemyMover = GetComponent<EnemyMover>();

        _stopDelay = 0.5f;
        _delayBetweenAttacks = 1.5f;

        _dieDelay = new WaitForSeconds(_stopDelay);
        _attackPhaseDuration = new WaitForSeconds(_delayBetweenAttacks);
        _canAttack = false;
        _attackPoint.AttackStateChanged += OnAttackStateChanged;
    }

    private void OnDisable()
    {
        _attackPoint.AttackStateChanged -= OnAttackStateChanged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
        {
            _enemyMover.Stop(_stopDelay);
            TakeDamage(bullet.DamagePerShot);
            DamageTaken?.Invoke();
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

            if (collider.TryGetComponent(out Attacker attacker))
            {
                attacker.TakeDamage(_attackDamage);
            }

            yield return _attackPhaseDuration;
        }
    }

    private void Die() 
    {
        _enemyMover.Stop(_stopDelay);
        Died?.Invoke();

        StartCoroutine(WaitDieDelay());
    }

    private IEnumerator WaitDieDelay()
    { 
        yield return _dieDelay;

        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
            Die();
    }
}
