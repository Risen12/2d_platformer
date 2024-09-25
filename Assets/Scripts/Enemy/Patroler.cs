using System.Collections.Generic;
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
    }

    private void OnPointReached(Transform point)
    {
        if (_patrolPoints.Contains(point))
        {
            HandleOnReachedPoint();
        }
    }

    private void HandleOnReachedPoint()
    {
        _enemyMover.ChangeDirection();
        _enemyMover.Stop(_patrolDelay);
    }

    private void onEnemyEntered(Vector2 playerPosition)
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (playerPosition - currentPosition).normalized;

        _enemyMover.ChangeMoveState(true);
        _enemyMover.ChangeDirection();
        _isChasing = true;
    }

    private void OnEnemyExited() 
    {
        _isChasing = false;
    }
}
