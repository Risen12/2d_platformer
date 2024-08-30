using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _timeIdle;
    [SerializeField] private Transform _rightPatrolBorderPoint;
    [SerializeField] private Transform _leftPatrolBorderPoint;

    private SpriteRenderer _spriteRenderer;
    private Vector2 _currentDirection;
    private bool _isMoving;
    private WaitForSeconds _patrol;

    public event Action<bool> Moved;
    public event Action StayEnded;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentDirection = Vector2.right;
        _isMoving = true;
        Moved?.Invoke(_isMoving);

        _patrol = new WaitForSeconds(_timeIdle);
    }

    private void Update()
    {
        if (_isMoving)
            Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PatrolPoint point))
        {
            OnPointReached(point.transform);
        }
    }

    public void Move()
    {
        _isMoving = true;
        Moved?.Invoke(_isMoving);

        transform.Translate(_currentDirection * _moveSpeed * Time.deltaTime);
    }

    public void Stop()
    {
        _isMoving = false;
        Moved?.Invoke(_isMoving);
    }

    public void ChangeDirection(Vector2 direction)
    {
        if (direction == Vector2.right)
        {
            _spriteRenderer.flipX = false;
            _currentDirection = Vector2.right;
        }
        else
        {
            _spriteRenderer.flipX = true;
            _currentDirection = Vector2.left;
        }
    }

    private IEnumerator WaitPatrol()
    {
        Stop();

        yield return _patrol;

        Move();
        StayEnded?.Invoke();
    }

    private void OnPointReached(Transform point)
    {
        Stay();

        if (point == _rightPatrolBorderPoint)
            ChangeDirection(Vector2.left);
        else
            ChangeDirection(Vector2.right);
    }

    private void Stay()
    {
        StartCoroutine(WaitPatrol());
    }

}
