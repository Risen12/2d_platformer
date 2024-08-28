using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Enemy))]
public class EnemyAnimatorController : MonoBehaviour
{
    private readonly int MoveParamHash = Animator.StringToHash("isMoving");
    private readonly int DieParamHash = Animator.StringToHash("Die");

    private Animator _animator;
    private Enemy _enemy;

    private void OnDisable()
    {
        _enemy.OnDie -= OnDie;
        _enemy.OnMoved -= OnMove;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();

        _enemy.OnDie += OnDie;
        _enemy.OnMoved += OnMove;
    }

    private void OnDie()
    {
        _animator.SetTrigger(DieParamHash);
    }

    private void OnMove(bool state)
    {
        _animator.SetBool(MoveParamHash, state);
    }
}
