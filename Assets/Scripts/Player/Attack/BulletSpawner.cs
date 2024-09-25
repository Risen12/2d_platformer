using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Mover _mover;
    [SerializeField] private int _bulletCount;

    private AudioSource _audioSource;
    private ObjectPool<Bullet> _bulletPool;
    private Vector2 _playerPosition;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: GetBullet,
            actionOnRelease: ReleaseBullet,
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: _bulletCount,
            maxSize: _bulletCount
        );

        _audioSource = GetComponent<AudioSource>();
    }

    public void PrepareBullet()
    {
        _bulletPool.Get();
        _audioSource.Play();
    }

    private void GetBullet(Bullet bullet)
    {
        _playerPosition = _mover.GetCurrentPosition();
        Vector2 playerDirection = _mover.GetCurrentDirection();

        bullet.transform.position = GetStartPosition();
        bullet.SetDirection(playerDirection);
        bullet.gameObject.SetActive(true);

        Vector2 direction;

        if (playerDirection == Vector2.left)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        bullet.AddForce(direction);

        bullet.CollisionHappened += OnBulletCollide;
    }

    private Bullet CreateBullet() => Instantiate(_bulletPrefab, _mover.GetCurrentPosition(), Quaternion.identity);

    private void OnBulletCollide(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.CollisionHappened -= OnBulletCollide;
        bullet.gameObject.SetActive(false);
    }

    private Vector3 GetStartPosition()
    {
        float offsetX = 0.3f;
        float offsetY = 0.4f;

        Vector3 startPosition;
        Vector2 currentDirection = _mover.GetCurrentDirection();

        if (currentDirection == Vector2.left)
            startPosition = new Vector3(_playerPosition.x - offsetX, _playerPosition.y + offsetY, 0f);
        else
            startPosition = new Vector3(_playerPosition.x + offsetX, _playerPosition.y + offsetY, 0f);

        return startPosition;
    }
}
