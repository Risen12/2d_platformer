using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(Enemy), typeof(Animator))]
[RequireComponent(typeof(Health))]
public class EnemyAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private EnemyMover _enemyMover;
    private Enemy _enemy;
    private Health _health;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyMover = GetComponent<EnemyMover>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Died += OnDied;
        _enemyMover.MoveStateChanged += OnMoveStateChanged;
        _enemy.Attacking += OnAttacked;
        _health.DamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
        _enemyMover.MoveStateChanged -= OnMoveStateChanged;
        _enemy.Attacking -= OnAttacked;
        _health.DamageTaken -= OnDamageTaken;
    }

    private void OnDied()
    {
        _animator.SetTrigger(EnemyAnimatorData.Params.DieParamHash);
    }

    private void OnMoveStateChanged(bool state)
    {
        _animator.SetBool(EnemyAnimatorData.Params.MoveParamHash, state);
    }

    private void OnDamageTaken(float _)
    {
        _animator.SetTrigger(EnemyAnimatorData.Params.HurtParamHash);
    }

    private void OnAttacked()
    {
        _animator.SetTrigger(EnemyAnimatorData.Params.AttackParamHash);
    }
}