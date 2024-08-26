using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class Shooter : MonoBehaviour
{
    private const string AttackTriggerParamName = "Attack";

    [SerializeField] private Bullet _bulletPrefab;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private ObjectPool<Bullet> _bulletPool;
    private int _bulletCount;
    private Coroutine _animationBeforeShot;

    private void OnDisable()
    {
        if (_animationBeforeShot != null)
            StopCoroutine(_animationBeforeShot);
    }

    private void Start()
    {
        _bulletCount = 30;
        _bulletPool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: GetBullet,
            actionOnRelease: ReleaseBullet,
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: _bulletCount,
            maxSize: _bulletCount
            );

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Shot();
    }

    private void Shot()
    {
        float animationTime = 0.37f;
        KeyCode fireKey = KeyCode.F;

        if (Input.GetKeyDown(fireKey))
        {
            _animator.SetTrigger(AttackTriggerParamName);

            if(_animationBeforeShot != null)
                StopCoroutine( _animationBeforeShot);

            _animationBeforeShot = StartCoroutine(ShotAfterAnimation(animationTime));
        }
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.transform.position = GetStartPosition();
        bullet.SetDirection(_spriteRenderer.flipX);
        bullet.gameObject.SetActive(true);

        Vector2 direction;
        if (_spriteRenderer.flipX)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        bullet.AddForce(direction);

        bullet.CollisionHappened += OnBulletCollide;
    }

    private Bullet CreateBullet() => Instantiate( _bulletPrefab, transform);

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.CollisionHappened -= OnBulletCollide;
        bullet.gameObject.SetActive(false);
    }

    private Vector3 GetStartPosition()
    {
        float offsetX = 0.3f;
        float offsetY = 0.4f;

        Vector3 startPosition = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
        return startPosition;
    }

    private void OnBulletCollide(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private IEnumerator ShotAfterAnimation(float delay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        yield return waitForSeconds;
        _bulletPool.Get();
    }
}
