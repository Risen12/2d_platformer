using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Shooter), typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerAnimatorController : MonoBehaviour
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private readonly int RunHash = Animator.StringToHash("isRunning");
    private readonly int MoveHash = Animator.StringToHash("isMoving");
    private readonly int OnGroundHash = Animator.StringToHash("OnGround");

    private Mover _mover;
    private Shooter _shooter;
    private Animator _animator;
    private bool _isOnGround;

    private void OnDisable()
    {
        _mover.OnJumped -= OnJumped;
        _mover.OnMoved -= OnMoved;
        _mover.OnRan -= OnRan;
        _mover.OnGroundStateChanged -= OnGroundStateChanged;

        _shooter.OnAttacked -= OnAttacked;
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _shooter = GetComponent<Shooter>();
        _animator = GetComponent<Animator>();

        _mover.OnJumped += OnJumped;
        _mover.OnMoved += OnMoved;
        _mover.OnRan += OnRan;
        _mover.OnGroundStateChanged += OnGroundStateChanged;

        _shooter.OnAttacked += OnAttacked;

        _isOnGround = true;
    }

    private void OnAttacked()
    {
        _animator.SetTrigger(AttackHash);
    }

    private void OnRan(bool state)
    {
        _animator.SetBool(RunHash, state);
    }

    private void OnMoved(bool state)
    {
        _animator.SetBool(MoveHash, state);      
    }

    private void OnJumped()
    {
        if (_isOnGround == false)
            return;

        _animator.SetTrigger(JumpHash);
    }

    private void OnGroundStateChanged(bool state)
    {
        _isOnGround = state;
        _animator.SetBool(OnGroundHash, _isOnGround);
    }
}
