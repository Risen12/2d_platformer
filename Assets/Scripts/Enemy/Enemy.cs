using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemyMover), typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer), typeof(Patroler))]
public class Enemy : MonoBehaviour, IDamagable
{
    private const float MinDamage = 10f;
    private const float MaxDamage = 20f;

    [SerializeField] private float _maxHealth = 100f;
    [Range(MinDamage, MaxDamage)]
    [SerializeField] private float _damagePerAttack = 10f;

    private EnemyMover _enemyMover;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private Patroler _patroler;
    private WaitForSeconds _delayBeforeDeath;
    private float _health;
    private bool _isAttacking;
    private WaitForSeconds _attackPhase;

    public event Action Died;
    public event Action Attacked;
    public event Action DamageTaken;

    private void Awake()
    {
        _enemyMover = GetComponent<EnemyMover>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _patroler = GetComponent<Patroler>();

        _patroler.CanAttack += Attack;

        float delay = 1f;
        _delayBeforeDeath = new WaitForSeconds(delay);

        _health = _maxHealth;
        _isAttacking = false;

        float attackPhaseDelay = 0.6f;
        _attackPhase = new WaitForSeconds(attackPhaseDelay);
    }

    private void OnDisable()
    {
        _patroler.CanAttack -= Attack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet bullet))
            TakeDamage(bullet.DamagePerShot);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker) && _isAttacking)
        {
            attacker.TakeDamage(_damagePerAttack);
        }
    }

    private void Die()
    {
        _enemyMover.Stop();

        Died?.Invoke();

        StartCoroutine(WaitDelayBeforeDeath());
    }

    private IEnumerator WaitDelayBeforeDeath()
    {
        yield return _delayBeforeDeath;

        gameObject.SetActive(false);
    }

    private void Attack()
    {
        Attacked?.Invoke();

        StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        _enemyMover.Stop();
        _isAttacking = true;
         
        yield return _attackPhase;

        Vector2 spriteSize = _spriteRenderer.size;
        _boxCollider2D.size = spriteSize;
        _isAttacking = false;

        _enemyMover.Move();
    }

    public void TakeDamage(float damage)
    {
        _enemyMover.Stop();
        DamageTaken?.Invoke();

        if (_health - damage > 0)
            _health -= damage;
        else
            Die();
    }
}
