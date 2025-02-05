using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class Patroler : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private VisibleZone _visibleZone;
    [SerializeField] private float _patrolDelay;

    private EnemyMover _enemyMover;
    private bool _isChasing;

    private void Awake()
    {
        _isChasing = false;
        _enemyMover = GetComponent<EnemyMover>();

        _visibleZone.EnemyEntered += onEnemyEntered;
        _visibleZone.EnemyExited += OnEnemyExited;
    }

    private void OnDisable()
    {
        _visibleZone.EnemyEntered -= onEnemyEntered;
        _visibleZone.EnemyExited -= OnEnemyExited;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isChasing == false)
        {
            if (collision.gameObject.TryGetComponent(out PatrolPoint patrolPoint))
            {
                OnPointReached(patrolPoint.transform);
            }
        }

        if (collision.gameObject.TryGetComponent(out MapBorder mapBorder))
        {
            _enemyMover.ChangeDirection();
        }
    }

    private void OnPointReached(Transform point)
    {
        if (_patrolPoints.Contains(point))
        {
            Transform nextPoint = _patrolPoints.Where(otherPoint => otherPoint != point).First();
            CalculatePathToNextPoint(point, nextPoint);
        }
    }

    private void CalculatePathToNextPoint(Transform currentPoint, Transform nextPoint)
    {
        _enemyMover.Stop(_patrolDelay);

        Vector2 direction = (nextPoint.transform.position - currentPoint.transform.position).normalized;

        VerifyChangeDirection(direction);
    }

    private void onEnemyEntered(Vector2 playerPosition)
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (playerPosition - currentPosition).normalized;

        _enemyMover.ChangeMoveState(true);

        VerifyChangeDirection(direction);

        _isChasing = true;
    }

    private void OnEnemyExited() 
    {
        _isChasing = false;
    }

    private void VerifyChangeDirection(Vector2 direction)
    {
        if (direction.x < 0 && transform.rotation == Quaternion.identity
    || direction.x > 0 && transform.rotation != Quaternion.identity)
        {
            _enemyMover.ChangeDirection();
        }
    }
}