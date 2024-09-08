using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private VisibleZone _visibleZone;
    [SerializeField] private AttackPoint _attackPoint;

    private Vector2 _currentDirection;
    private WaitForSeconds _stopDelay;
    private bool _isMoving;

    public event Action<bool> MoveStateChanged;

    private void Awake()
    {
        _currentDirection = Vector2.right;
        ChangeMoveState(true);
    }

    private void Update()
    {
        if (_isMoving)
            Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out MapBorder mapBorder))
        {
            ChangeDirection(_currentDirection * -1);
        }
    }

    private void Move()
    {
        transform.Translate(_currentDirection * _moveSpeed * Time.deltaTime);
    }

    private IEnumerator WaitStopDelay()
    {
        ChangeMoveState(false);

        yield return _stopDelay;

        ChangeMoveState(true);
    }

    public void ChangeMoveState(bool state)
    {
        MoveStateChanged?.Invoke(state);
        _isMoving = state;
    }

    public void Stop(float delay = 0f)
    {
        if (delay > 0f)
        {
            _stopDelay = new WaitForSeconds(delay);
            StartCoroutine(WaitStopDelay());
        }
        else
        {
            ChangeMoveState(false);
        }
    }

    public void ChangeDirection(Vector2 direction)
    {
        float rotationY = -180;
        Quaternion leftRotation = Quaternion.Euler(0, rotationY, 0);

        if (direction.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = leftRotation;
        }
    }
}
