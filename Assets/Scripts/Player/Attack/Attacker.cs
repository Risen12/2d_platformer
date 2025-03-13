using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(GroundDetector))]
public class Attacker : MonoBehaviour
{
    [SerializeField] private BulletSpawner _bulletSpawner;
    [SerializeField] private InputReader _inputReader;

    private GroundDetector _groundDetector;
    private Mover _mover;
    private Coroutine _animationBeforeShot;
    private WaitForSeconds _delayBeforeShot;

    public event Action Attacked;

    private void Awake()
    {
        float delay = 0.37f;
        _delayBeforeShot = new WaitForSeconds(delay);

        _mover = GetComponent<Mover>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void OnEnable()
    {
        _inputReader.AttackButtonPressed += OnAttackButtonPressed;
    }

    private void OnDisable()
    {
        _inputReader.AttackButtonPressed -= OnAttackButtonPressed;

        if (_animationBeforeShot != null)
            StopCoroutine(_animationBeforeShot);
    }

    private void OnAttackButtonPressed()
    {
        if (_groundDetector.IsGrounded && _mover.IsMoving == false)
        {
            Attacked?.Invoke();

            if (_animationBeforeShot != null)
                StopCoroutine(_animationBeforeShot);

            _animationBeforeShot = StartCoroutine(ShotAfterAnimation());
        }
    }

    private IEnumerator ShotAfterAnimation()
    {
        yield return _delayBeforeShot;
        _bulletSpawner.PrepareBullet();
    }
}