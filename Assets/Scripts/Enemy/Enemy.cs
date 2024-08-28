using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _rightPatrolBorderPoint;
    [SerializeField] private Transform _leftPatrolBorderPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _timeForIdle;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Vector2 _currentDirection;
    private bool _isMoving;
    private Coroutine _patrolCoroutine;
    private WaitForSeconds _patrol;
    private WaitForSeconds _delayBeforeDeath;

    public event Action<bool> OnMoved;
    public event Action OnDie;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();

        float delay = 1f;
        _patrol = new WaitForSeconds(_timeForIdle);
        _delayBeforeDeath = new WaitForSeconds(delay);
        
        _currentDirection = Vector2.right;
        _isMoving = true;
        OnMoved?.Invoke(_isMoving);
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PatrolPoint point))
        {
            OnPointReached(point.transform);
        }

        if (collision.gameObject.TryGetComponent(out Bullet bullet))
        {
            Die();
        }
    }

    private IEnumerator WaitPatrol()
    {
        _isMoving = false;
        OnMoved?.Invoke(_isMoving);

        yield return _patrol;

        _isMoving = true;
        OnMoved?.Invoke(_isMoving);
    }

    private IEnumerator WaitDelayBeforeDeath()
    {
        yield return _delayBeforeDeath;

        gameObject.SetActive(false);
    }

    private void Move()
    {
        if (_isMoving)
            transform.Translate(_currentDirection * _moveSpeed * Time.deltaTime);
    }

    private void Die()
    {
        float colliderSizeY = 0.25f;
        _isMoving = false;
        OnMoved?.Invoke(_isMoving);

        if (_patrolCoroutine != null)
            StopCoroutine(_patrolCoroutine);

        OnDie?.Invoke();
        _boxCollider.size = new Vector2(_boxCollider.size.x, colliderSizeY);

        StartCoroutine(WaitDelayBeforeDeath());
    }

    private void OnPointReached(Transform point)
    {
        _patrolCoroutine = StartCoroutine(WaitPatrol());

        if (point == _rightPatrolBorderPoint)
            ChangeDirection(Vector2.left);
        else
            ChangeDirection(Vector2.right);
    }

    private void ChangeDirection(Vector2 direction)
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
}
