using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Transform _bulletSpawnPlace;

    private AudioSource _audioSource;
    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        int bulletCount = 30;

        _bulletPool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: GetBullet,
            actionOnRelease: ReleaseBullet,
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: bulletCount,
            maxSize: bulletCount
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
        float currentPlayerRotation = _rotator.CurrentRotationY;

        bullet.transform.SetParent(_bulletSpawnPlace);
        bullet.transform.localPosition = Vector3.zero;
        bullet.gameObject.SetActive(true);

        Vector2 direction;

        if (currentPlayerRotation == _rotator.LeftRotationY || currentPlayerRotation == -_rotator.LeftRotationY)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        bullet.AddForce(direction);
        bullet.RotateToDirection(_rotator.CurrentRotationY);
        bullet.CollisionHappened += OnBulletCollide;
    }

    private Bullet CreateBullet() => Instantiate(_bulletPrefab, _bulletSpawnPlace);

    private void OnBulletCollide(Bullet bullet) => _bulletPool.Release(bullet);

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.CollisionHappened -= OnBulletCollide;
        bullet.gameObject.SetActive(false);
    }
}