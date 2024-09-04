using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _leftMapBorder;
    [SerializeField] private float _rightMapBorder;

    private SpriteRenderer _spriteRenderer;
    private Vector2 _currentDirection;
    private bool _isMoving;

    public event Action<bool> Moved;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentDirection = Vector2.right;

        ChangeMoveState(true);
    }

    private void Update()
    {
        if (_isMoving)
            Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out MapBorder mapBorder))
        {
            if (_spriteRenderer.flipX)
                ChangeDirection(Vector2.right);
            else
                ChangeDirection(Vector2.left);
        }
    }

    private void ChangeMoveState(bool state)
    {
        _isMoving = state;
        Moved?.Invoke(_isMoving);
    }

    public void Move()
    {
        ChangeMoveState(true);

        transform.Translate(_currentDirection * _moveSpeed * Time.deltaTime);
    }

    public void Stop()
    {
        ChangeMoveState(false);
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

    public void MoveToPlayer(Vector2 direction)
    {
        ChangeMoveState(true);
        ChangeDirection(direction);
    }

    public bool GetRotation() => _spriteRenderer.flipX;
}
