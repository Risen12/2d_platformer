using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private AttackPoint _attackPoint;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private float _delayBetweenAttacks;
    [SerializeField] private float _attackDamage;

    private float _health;
    private EnemyMover _enemyMover;
    private WaitForSeconds _dieDelay;
    private WaitForSeconds _attackDelay;
    private bool _canAttack;

    public event Action Attacking;
    public event Action DamageTaken;
    public event Action Died;

    private void Awake()
    {
        _health = _startHealth;
        _enemyMover = GetComponent<EnemyMover>();

        _canAttack = true;

        float delay = 0.5f;
        _dieDelay = new WaitForSeconds(delay);
        _attackDelay = new WaitForSeconds(_delayBetweenAttacks);

        _attackPoint.CanAttack += Attack;
    }

    private void OnDisable()
    {
        _attackPoint.CanAttack -= Attack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
        {
            _enemyMover.Stop(0.5f);
            TakeDamage(bullet.DamagePerShot);
            DamageTaken?.Invoke();
        }
    }

    private IEnumerator WaitBeforeAttack()
    {
        _canAttack = false;

        yield return _attackDelay;

        _canAttack = true;
    }

    private void Attack()
    {
        if (_canAttack)
        {
            Attacking?.Invoke();

            Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.transform.position, _attackRadius, _playerLayerMask);

            if (playerCollider.gameObject.TryGetComponent(out Attacker attacker))
            {
                attacker.TakeDamage(_attackDamage);
            }

            StartCoroutine(WaitBeforeAttack());
        }
    }

    private void Die() 
    {
        _enemyMover.Stop(0.5f);
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
