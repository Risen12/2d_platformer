using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private Transform _groundVerifier;
    [SerializeField] private float _delayAfterAttack;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _platformLayer;

    private Rigidbody2D _rigidbody;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _isMoving;
    private bool _isBlockAfterAttack;
    private WaitForSeconds _afterAttackDelay;
    private float _leftRotationY;
    private Vector2 _currentDirection;

    public event Action Jumped;
    public event Action<bool> Moved;
    public event Action<bool> Ran;
    public event Action<bool> GroundStateChanged;

    public bool IsGrounded => _isGrounded;
    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _isRunning = false;
        _isGrounded = true;
        _isBlockAfterAttack = false;
        _leftRotationY = -180f;
        _currentDirection = Vector2.right;

        _afterAttackDelay = new WaitForSeconds(_delayAfterAttack);
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        VerifyGroundedState();
    }

    private void Move()
    {
        if (_isBlockAfterAttack)
            return;

        float axis = Input.GetAxisRaw(HorizontalAxis);

        if (axis > 0)
        {
            MoveStateChanged(true);
            ChangeDirection(Vector2.right);
        }
        else if (axis < 0)
        {
            MoveStateChanged(true);
            ChangeDirection(Vector2.left);
        }
        else
        {
            MoveStateChanged(false);
        }

        if(_isGrounded)
            Run();

        if (_isRunning)
        {
            MoveWithSpeed(_runSpeed, axis);
        }
        else
        {
            MoveWithSpeed(_walkSpeed, axis);
        }
    }

    private void MoveWithSpeed(float speed, float axis)
    {
        transform.Translate(_currentDirection * axis * speed * Time.deltaTime);
    }

    private void Run()
    {
        KeyCode runButton = KeyCode.LeftShift;

        if (Input.GetKey(runButton))
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        Ran?.Invoke(_isRunning);
    }

    private void Jump()
    {
        if (_isBlockAfterAttack)
            return;

        float axis = Input.GetAxisRaw(VerticalAxis);

        if (axis > 0)
        {
            if (VerifyGroundedState())
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
                Jumped?.Invoke();
            }
        }
    }

    private void MoveStateChanged(bool state)
    {
        _isMoving = state;
        Moved?.Invoke(state);
    }

    private void ChangeDirection(Vector2 direction)
    {
        Quaternion leftRotation = Quaternion.Euler(0, _leftRotationY, 0);

        if (direction.x > 0)
        {
            _currentDirection = Vector2.right;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            _currentDirection = Vector2.left;
            transform.rotation = leftRotation;
        }
    }

    private bool VerifyGroundedState()
    {
        float colliderSizeX = 0.4f;
        float colliderSizeY = 0.1f;

        _isGrounded = Physics2D.OverlapCapsule(_groundVerifier.position, new Vector2(colliderSizeX, colliderSizeY), CapsuleDirection2D.Horizontal, 0f, _groundLayer) ||
        Physics2D.OverlapCapsule(_groundVerifier.position, new Vector2(colliderSizeX, colliderSizeY), CapsuleDirection2D.Horizontal, 0f, _platformLayer);

        GroundStateChanged?.Invoke(_isGrounded);

        return _isGrounded;
    }

    private IEnumerator StandAfterAttack()
    {
        MoveStateChanged(false);
        _isBlockAfterAttack = true;

        yield return _afterAttackDelay;

        _isBlockAfterAttack = false;
        MoveStateChanged(true);
    }

    public Vector2 GetCurrentDirection() => _currentDirection;

    public Vector2 GetCurrentPosition() => transform.position;

    public void Stop(bool isAfterAttack)
    {
        if (isAfterAttack)
            StartCoroutine(StandAfterAttack());
        else
            _isMoving = false;
    }
}
