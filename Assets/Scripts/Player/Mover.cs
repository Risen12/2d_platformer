using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private Rotator _rotator;

    private bool _isMoving;
    private Rigidbody2D _rigidbody;

    public event Action<bool> Moved;
    public event Action<bool> Ran;
    public event Action Jumped;

    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float direction, bool isRunning = false)
    {
        MoveStateChanged(true);

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
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpSpeed);
        Jumped?.Invoke();
    }

    public void Stop()
    {
        Ran?.Invoke(false);
        MoveStateChanged(false);
    }

    private void MoveWithSpeed(float speed)
    {
        if (_rotator.CurrentRotationY == 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void MoveStateChanged(bool state)
    {
        _isMoving = state;
        Moved?.Invoke(state);
    }
}