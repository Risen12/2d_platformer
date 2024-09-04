using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(Animator))]
public class Patroler : MonoBehaviour
{
    [SerializeField] private Transform _rightPatrolBorderPoint;
    [SerializeField] private Transform _leftPatrolBorderPoint;
    [SerializeField] private float _timeIdle;
    [SerializeField] private VisibleZone _visibleZone;

    private WaitForSeconds _patrol;
    private EnemyMover _mover;
    private bool _isChasing;

    public event Action CanAttack;

    private void Awake()
    {
        _patrol = new WaitForSeconds(_timeIdle);
        _mover = GetComponent<EnemyMover>();
    }

    private void OnEnable()
    {
        _visibleZone.EnemyExited += StopChasePlayer;
        _visibleZone.EnemyEntered += ChasePlayer;
    }

    private void OnDisable()
    {
        _visibleZone.EnemyExited -= StopChasePlayer;
        _visibleZone.EnemyEntered -= ChasePlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PatrolPoint point))
        {
            if(_isChasing == false)
                OnPointReached(point.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
        {
            CanAttack?.Invoke();
        }
    }

    private IEnumerator WaitPatrol()
    {
        _mover.Stop();

        yield return _patrol;

        _mover.Move();
    }

    private void OnPointReached(Transform point)
    {
        Stay();

        if (point == _rightPatrolBorderPoint)
            _mover.ChangeDirection(Vector2.left);
        else
            _mover.ChangeDirection(Vector2.right);

        _visibleZone.Rotate(_mover.GetRotation());
    }

    private void Stay()
    {
        StartCoroutine(WaitPatrol());
    }

    private void ChasePlayer(Vector2 direction)
    {
        _visibleZone.Rotate(_mover.GetRotation());
        _mover.MoveToPlayer(direction);
        _isChasing = true;
    }

    private void StopChasePlayer()
    {
        _isChasing = false;
    }
}
