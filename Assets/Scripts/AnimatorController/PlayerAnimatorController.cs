using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Attacker), typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D), typeof(GroundDetector), typeof(Health))]
public class PlayerAnimatorController : MonoBehaviour
{
    private readonly int AttackParamHash = Animator.StringToHash("Attack");
    private readonly int JumpParamHash = Animator.StringToHash("Jump");
    private readonly int RunParamHash = Animator.StringToHash("isRunning");
    private readonly int MoveParamHash = Animator.StringToHash("isMoving");
    private readonly int OnGroundParamHash = Animator.StringToHash("OnGround");
    private readonly int HurtParamHash = Animator.StringToHash("IsAttacked");
    private readonly int DieParamHash = Animator.StringToHash("Died");

    private Mover _mover;
    private GroundDetector _groundDetector;
    private Attacker _attacker;
    private Animator _animator;
    private Health _health;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _health = GetComponent<Health>();
        _attacker = GetComponent<Attacker>();
        _animator = GetComponent<Animator>();
        _groundDetector = GetComponent<GroundDetector>();

        _mover.Jumped += OnJumped;
        _mover.Moved += OnMoved;
        _mover.Ran += OnRan;
        _groundDetector.GroundStateChanged += OnGroundStateChanged;

        _attacker.Attacked += OnAttacked;
        _health.DamageTaken += OnDamageTaken;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _mover.Jumped -= OnJumped;
        _mover.Moved -= OnMoved;
        _mover.Ran -= OnRan;
        _groundDetector.GroundStateChanged -= OnGroundStateChanged;

        _attacker.Attacked -= OnAttacked;
        _health.DamageTaken -= OnDamageTaken;
        _health.Died -= OnDied;
    }

    private void OnAttacked()
    {
        _animator.SetTrigger(AttackParamHash);
    }

    private void OnRan(bool state)
    {
        _animator.SetBool(RunParamHash, state);
    }

    private void OnMoved(bool state)
    {
        _animator.SetBool(MoveParamHash, state);      
    }

    private void OnJumped()
    {
        _animator.SetTrigger(JumpParamHash);
    }

    private void OnDamageTaken()
    {
        _animator.SetTrigger(HurtParamHash);
    }

    private void OnGroundStateChanged(bool state)
    {
        _animator.SetBool(OnGroundParamHash, state);
    }

    private void OnDied()
    {
        _animator.SetTrigger(DieParamHash);
    }
}
