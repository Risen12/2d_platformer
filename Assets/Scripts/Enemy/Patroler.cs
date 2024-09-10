using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class Patroler : MonoBehaviour
{
    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;
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
        if (point.localPosition == _leftPatrolPoint.localPosition)
        {
            HandleOnReachedPoint(Vector2.right);
        }

        if (point.localPosition == _rightPatrolPoint.localPosition)
        {
            HandleOnReachedPoint(Vector2.left);
        }
    }

    private void HandleOnReachedPoint(Vector2 nextDirection)
    {
        _enemyMover.ChangeDirection(nextDirection);
        _enemyMover.Stop(_patrolDelay);
    }

    private void onEnemyEntered(Vector2 playerPosition)
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (playerPosition - currentPosition).normalized;

        _enemyMover.ChangeMoveState(true);
        _enemyMover.ChangeDirection(direction);
        _isChasing = true;
    }

    private void OnEnemyExited() 
    {
        _isChasing = false;
    }
}
