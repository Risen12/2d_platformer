using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource), typeof(Mover))]
public class Attacker: MonoBehaviour , IDamagable
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _maxHealth = 100f;

    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private Mover _mover;
    private ObjectPool<Bullet> _bulletPool;
    private int _bulletCount;
    private Coroutine _animationBeforeShot;
    private WaitForSeconds _delayBeforeShot;
    private float _health;

    public event Action OnAttacked;
    public event Action DamageTaken;

    private void OnDisable()
    {
        if (_animationBeforeShot != null)
            StopCoroutine(_animationBeforeShot);
    }

    private void Awake()
    {
        float delay = 0.37f;
        _delayBeforeShot = new WaitForSeconds(delay);
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

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _mover = GetComponent<Mover>();

        _health = _maxHealth;
    }

    private void Update()
    {
        Shot();
    }

    private void Shot()
    {
        KeyCode attackButton = KeyCode.F;

        if (Input.GetKeyDown(attackButton) && _mover.IsGrounded && _mover.IsMoving == false)
        {
            OnAttacked?.Invoke();

            if(_animationBeforeShot != null)
                StopCoroutine( _animationBeforeShot);

            _animationBeforeShot = StartCoroutine(ShotAfterAnimation());
        }
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.transform.position = GetStartPosition(_spriteRenderer.flipX);
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

    private Vector3 GetStartPosition(bool flipX)
    {
        float offsetX = 0.3f;
        float offsetY = 0.4f;

        Vector3 startPosition;
        if (flipX)
            startPosition = new Vector3(transform.position.x - offsetX, transform.position.y + offsetY, transform.position.z);
        else
            startPosition = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);

        return startPosition;
    }

    private void OnBulletCollide(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private IEnumerator ShotAfterAnimation()
    {
        yield return _delayBeforeShot;
        _bulletPool.Get();
        _audioSource.Play();
    }

    private void Die()
    { 
        
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Ударили");

        if (_health - damage > 0)
            _health -= damage;
        else
            Die();

        DamageTaken?.Invoke();
    }
}
