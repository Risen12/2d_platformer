using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(Enemy), typeof(Animator))]
public class EnemyAnimatorController : MonoBehaviour
{
    private readonly int MoveParamHash = Animator.StringToHash("isMoving");
    private readonly int DieParamHash = Animator.StringToHash("Die");
    private readonly int AttackParamHash = Animator.StringToHash("Attack");
    private readonly int HurtParamHash = Animator.StringToHash("IsAttacked");

    private Animator _animator;
    private EnemyMover _enemyMover;
    private Enemy _enemy;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyMover = GetComponent<EnemyMover>();

        _enemy.Died += OnDied;
        _enemyMover.MoveStateChanged += OnMoveStateChanged;
        _enemy.Attacking += OnAttacked;
        _enemy.DamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        _enemy.Died -= OnDied;
        _enemyMover.MoveStateChanged -= OnMoveStateChanged;
        _enemy.Attacking -= OnAttacked;
        _enemy.DamageTaken -= OnDamageTaken;
    }

    private void OnDied()
    {
        _animator.SetTrigger(DieParamHash);
    }

    private void OnMoveStateChanged(bool state)
    {
        _animator.SetBool(MoveParamHash, state);
    }

    private void OnDamageTaken()
    {
        _animator.SetTrigger(HurtParamHash);
    }

    private void OnAttacked()
    {
        _animator.SetTrigger(AttackParamHash);
    }
}
