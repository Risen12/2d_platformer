using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Mover), typeof(GroundDetector), typeof(Health))]
[RequireComponent(typeof(PlayerRotator))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _delayAfterAttack;
    [SerializeField] private MySceneManager _sceneManager;
    [SerializeField] private List<Teleport> _teleporControllers;

    private Health _health;
    private bool _isBlockAfterAttack;
    private WaitForSeconds _afterAttackDelay;
    private Mover _mover;
    private PlayerRotator _playerRotator;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _groundDetector = GetComponent<GroundDetector>();
        _mover = GetComponent<Mover>();
        _playerRotator = GetComponent<PlayerRotator>();
        _health = GetComponent<Health>();

        _isBlockAfterAttack = false;
        _afterAttackDelay = new WaitForSeconds(_delayAfterAttack);
    }

    private void OnEnable()
    {
        _health.DamageTaken += OnDamageTaken;
        _health.AfterDied += OnDied;

        foreach (Teleport teleporController in _teleporControllers)
            teleporController.Teleported += OnTeleported;
    }

    private void OnDisable()
    {
        _health.DamageTaken -= OnDamageTaken;
        _health.AfterDied -= OnDied;

        foreach (Teleport teleporController in _teleporControllers)
            teleporController.Teleported -= OnTeleported;
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

                _playerRotator.ChangeDirection(_inputReader.HorizontalDirection);
            }
            else
            {
                _mover.Stop();
            }

            if (_inputReader.VerticalDirection > 0 && _groundDetector.IsGrounded)
            {
                _mover.Jump();
            }
        }
    }

    private void OnDamageTaken(float _)
    {
        _mover.Stop();

        StartCoroutine(StandAfterAttack());
    }

    private void OnTeleported(Vector2 position)
    { 
        transform.position = position;
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