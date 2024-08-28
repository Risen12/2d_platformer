using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string GroundLayerName = "Ground";

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _runSpeed;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private bool _isRunning;
    private bool _isGrounded;

    public event Action<bool> OnGroundStateChanged;
    public event Action OnJumped;
    public event Action<bool> OnMoved;
    public event Action<bool> OnRan;

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
            OnMoved?.Invoke(false);
        }

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

        OnRan?.Invoke(_isRunning);
    }

    private void Jump()
    {
        float axis = Input.GetAxisRaw(VerticalAxis);

        if (axis > 0)
        {
            if (_isGrounded)
            {
                _rigidbody.AddForce(Vector2.up * _jumpSpeed);
                OnJumped?.Invoke();
            }
        }
    }

    private void OnMovedStarted(bool FlipX)
    {
        OnMoved?.Invoke(true);
        _spriteRenderer.flipX = FlipX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(GroundLayerName))
        {
            OnGroundedStateChanged(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(GroundLayerName))
        {
            OnGroundedStateChanged(false);
        }
    }

    private void OnGroundedStateChanged(bool state)
    { 
        _isGrounded = state;
        OnGroundStateChanged?.Invoke(state);
    }
}
