using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(Enemy), typeof(Animator))]
public class EnemyAnimatorController : MonoBehaviour
{
    private readonly int MoveParamHash = Animator.StringToHash("isMoving");
    private readonly int DieParamHash = Animator.StringToHash("Die");

    private Animator _animator;
    private EnemyMover _enemyMover;
    private Enemy _enemy;

    private void OnDisable()
    {
        _enemy.Died -= OnDied;
        _enemyMover.Moved -= OnMove;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyMover = GetComponent<EnemyMover>();

        _enemy.Died += OnDied;
        _enemyMover.Moved += OnMove;
    }

    private void OnDied()
    {
        _animator.SetTrigger(DieParamHash);
    }

    private void OnMove(bool state)
    {
        _animator.SetBool(MoveParamHash, state);
    }
}
