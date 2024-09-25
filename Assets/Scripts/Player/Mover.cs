using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpSpeed;

    private bool _isMoving;
    private float _leftRotationY;
    private Rigidbody2D _rigidbody;

    public event Action<bool> Moved;
    public event Action<bool> Ran;
    public event Action Jumped;

    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _leftRotationY = -180f;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float direction, bool isRunning = false)
    {
        MoveStateChanged(true);
        ChangeDirection(direction);

        if (isRunning)
        {
            Ran?.Invoke(true);
            MoveWithSpeed(_runSpeed);
        }
        else
        {
            Ran?.Invoke(false);
            MoveWithSpeed(_walkSpeed);
        }
    }

    public void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
        Jumped?.Invoke();
    }

    public Vector2 GetCurrentPosition() => transform.position;

    public Vector2 GetCurrentDirection()
    {
        if (transform.rotation.y != 0)
            return Vector2.left;
        else
            return Vector2.right;
    }

    public void Stop()
    {
        Ran?.Invoke(false);
        MoveStateChanged(false);
    }

    private void MoveWithSpeed(float speed)
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void MoveStateChanged(bool state)
    {
        _isMoving = state;
        Moved?.Invoke(state);
    }

    private void ChangeDirection(float direction)
    {
        Quaternion leftRotation = Quaternion.Euler(0, _leftRotationY, 0);

        if (direction > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = leftRotation;
        }
    }
}
