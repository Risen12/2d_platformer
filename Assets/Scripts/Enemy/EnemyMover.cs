using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private VisibleZone _visibleZone;
    [SerializeField] private AttackPoint _attackPoint;
    [SerializeField] private MapBorder _leftMapBorder;
    [SerializeField] private MapBorder _rightMapBorder;

    private WaitForSeconds _stopDelay;
    private bool _isMoving;

    public event Action<bool> MoveStateChanged;

    private void Awake()
    {
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
            ChangeDirection();
        }
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

    public void ChangeDirection()
    {
        float leftRotationY = -180;
        Quaternion leftRotation = Quaternion.Euler(0, leftRotationY, 0);

        if (transform.rotation == Quaternion.identity)
        {
            transform.rotation = leftRotation;
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime);
    }

    private IEnumerator WaitStopDelay()
    {
        ChangeMoveState(false);

        yield return _stopDelay;

        ChangeMoveState(true);
    }
}
