using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private Transform _groundVerifier;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _platformLayer;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _isMoving;

    public event Action Jumped;
    public event Action<bool> Moved;
    public event Action<bool> Ran;
    public event Action<bool> GroundStateChanged;

    public bool IsGrounded => _isGrounded;
    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isRunning = false;
        _isGrounded = true;
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
        float axis = Input.GetAxisRaw(HorizontalAxis);

        if (axis > 0)
        {
            OnMovedStarted(false);
        }
        else if (axis < 0)
        {
            OnMovedStarted(true);
        }
        else
        {
            _isMoving = false;
            Moved?.Invoke(false);
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
        transform.Translate(Vector2.right * axis * speed * Time.deltaTime);
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
        float axis = Input.GetAxisRaw(VerticalAxis);

        if (axis > 0)
        {
            VerifyGroundedState();

            if (_isGrounded)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
                Jumped?.Invoke();
            }
        }
    }

    private void OnMovedStarted(bool FlipX)
    {
        _isMoving = true;
        Moved?.Invoke(_isMoving);
        _spriteRenderer.flipX = FlipX;
    }

    private void VerifyGroundedState()
    {
        float colliderSizeX = 0.4f;
        float colliderSizeY = 0.1f;

        _isGrounded = Physics2D.OverlapCapsule(_groundVerifier.position, new Vector2(colliderSizeX, colliderSizeY), CapsuleDirection2D.Horizontal, 0f, _groundLayer) ||
        Physics2D.OverlapCapsule(_groundVerifier.position, new Vector2(colliderSizeX, colliderSizeY), CapsuleDirection2D.Horizontal, 0f, _platformLayer);

        GroundStateChanged?.Invoke(_isGrounded);
    }


    public Vector2 GetCurrentDirection()
    { 
        float directionX = Input.GetAxisRaw(HorizontalAxis);
        float directionY = Input.GetAxisRaw(VerticalAxis);

        return new Vector2(directionX, directionY);
    }
}
