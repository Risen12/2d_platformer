using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Attacker _attacker;
    [SerializeField] private Health _health;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
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