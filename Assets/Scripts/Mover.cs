using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class Mover : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string RunAnimatorTriggerName = "isRunning";
    private const string MoveAnimatorTriggerName = "isMoving";
    private const string OnGroundParameterName = "OnGround";
    private const string JumpTriggerName = "Jump";
    private const int GroundLayerNumber = 6;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _runSpeed;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private bool _isGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _isGrounded = true;
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GroundLayerNumber)
        {
            _isGrounded = true;
            _animator.SetBool(OnGroundParameterName, _isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GroundLayerNumber)
        {
            _isGrounded = false;
            _animator.SetBool(OnGroundParameterName, _isGrounded);
        }

    }

    private void Move()
    {
        float axis = Input.GetAxisRaw(HorizontalAxis);

        if (axis > 0)
        {
            _animator.SetBool(MoveAnimatorTriggerName, true);
            _spriteRenderer.flipX = false;
        }
        else if (axis < 0)
        {
            _animator.SetBool(MoveAnimatorTriggerName, true);
            _spriteRenderer.flipX = true;
        }
        else
        {
            _animator.SetBool(MoveAnimatorTriggerName, false);
        }

        Run();

        if (_animator.GetBool(RunAnimatorTriggerName))
        {
            transform.Translate(Vector2.right * axis * _runSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * axis * _walkSpeed * Time.deltaTime);
        }
    }

    private void Run()
    {
        KeyCode shift = KeyCode.LeftShift;

        if (Input.GetKey(shift))
        {
            _animator.SetBool(RunAnimatorTriggerName, true);
        }
        else
        {
            _animator.SetBool(RunAnimatorTriggerName, false);
        }
    }

    private void Jump()
    {
        float axis = Input.GetAxisRaw(VerticalAxis);

        if (axis > 0)
        {
            if (_isGrounded)
            {
                _animator.SetTrigger(JumpTriggerName);
                _rigidbody2D.AddForce(Vector2.up * _jumpSpeed);
            }
        }
    }
}
