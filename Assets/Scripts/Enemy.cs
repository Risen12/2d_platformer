using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    private const string MoveAnimatorBoolName = "isMoving";
    private const string DieTriggerName = "Die";

    [SerializeField] private Transform _rightPatrolBorderPoint;
    [SerializeField] private Transform _leftPatrolBorderPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _timeForIdle;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Vector2 _currentDirection;
    private bool _isMoving;
    private Coroutine _waitPatrolCoroutine;
    private Coroutine _waitCoroutine;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _currentDirection = Vector2.right;
        _isMoving = true;
        _animator.SetBool(MoveAnimatorBoolName, _isMoving);
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
        WaitForSeconds wait = new WaitForSeconds(_timeForIdle);

        _isMoving = false;
        _animator.SetBool(MoveAnimatorBoolName, _isMoving);

        yield return wait;

        _isMoving = true;
        _animator.SetBool(MoveAnimatorBoolName, _isMoving);

        yield break;
    }

    private IEnumerator Wait()
    {
        float delayDie = 1f;
        WaitForSeconds wait = new WaitForSeconds(delayDie);

        yield return wait;

        gameObject.SetActive(false);

        yield break;
    }

    private void Move()
    {
        if (_isMoving)
        {
            transform.Translate(_currentDirection * _moveSpeed * Time.deltaTime);
        }
    }

    private void Die()
    {
        float colliderSizeY = 0.25f;
        _isMoving = false;
        _animator.SetBool(MoveAnimatorBoolName, _isMoving);

        if (_waitPatrolCoroutine != null)
        {
            StopCoroutine(_waitPatrolCoroutine);
        }

        _animator.SetTrigger(DieTriggerName);
        _boxCollider.size = new Vector2(_boxCollider.size.x, colliderSizeY);

        _waitCoroutine = StartCoroutine(Wait());
    }

    private void OnPointReached(Transform point)
    {
        _waitPatrolCoroutine = StartCoroutine(WaitPatrol());

        if (point == _rightPatrolBorderPoint)
        {
            ChangeDirection(Vector2.left);
        }
        else
        {
            ChangeDirection(Vector2.right);
        }
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
