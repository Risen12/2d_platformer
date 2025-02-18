using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Attacker), typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D), typeof(GroundDetector), typeof(Health))]
public class PlayerAnimatorController : MonoBehaviour
{
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
        _animator.SetTrigger(PlayerAnimatorData.Params.AttackParamHash);
    }

    private void OnRan(bool state)
    {
        _animator.SetBool(PlayerAnimatorData.Params.RunParamHash, state);
    }

    private void OnMoved(bool state)
    {
        _animator.SetBool(PlayerAnimatorData.Params.MoveParamHash, state);      
    }

    private void OnJumped()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.JumpParamHash);
    }

    private void OnDamageTaken(float _)
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.HurtParamHash);
    }

    private void OnGroundStateChanged(bool state)
    {
        _animator.SetBool(PlayerAnimatorData.Params.OnGroundParamHash, state);
    }

    private void OnDied()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.DieParamHash);
    }
}