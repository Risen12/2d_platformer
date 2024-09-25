using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mover), typeof(GroundDetector), typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _delayAfterAttack;
    [SerializeField] MySceneManager _sceneManager;

    private Health _health;
    private bool _isBlockAfterAttack;
    private WaitForSeconds _afterAttackDelay;
    private Mover _mover;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _groundDetector = GetComponent<GroundDetector>();
        _mover = GetComponent<Mover>();
        _health = GetComponent<Health>();

        _isBlockAfterAttack = false;
        _afterAttackDelay = new WaitForSeconds(_delayAfterAttack);

        _health.DamageTaken += OnDamageTaken;
        _health.AfterDied += OnDied;
    }

    private void OnDisable()
    {
        _health.DamageTaken -= OnDamageTaken;
        _health.AfterDied -= OnDied;
    }

    private void FixedUpdate()
    {
        if (_isBlockAfterAttack == false)
        {
            if (_inputReader.HorizontalDirection != 0)
            {
                if (_inputReader.IsRunning)
                {
                    _mover.Move(_inputReader.HorizontalDirection, true);
                }
                else
                {
                    _mover.Move(_inputReader.HorizontalDirection);
                }
            }
            else
            {
                _mover.Stop();
            }

            if (_inputReader.VerticalDirection != 0 && _groundDetector.IsGrounded)
            {
                _mover.Jump();
            }
        }
    }

    private void OnDamageTaken()
    {
        _mover.Stop();

        StartCoroutine(StandAfterAttack());
    }

    private void OnDied()
    {
        _mover.Stop();

        _sceneManager.ReloadLevel();
    }

    private IEnumerator StandAfterAttack()
    {
        _isBlockAfterAttack = true;

        yield return _afterAttackDelay;

        _isBlockAfterAttack = false;
    }
}
