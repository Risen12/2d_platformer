using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private float _speed;
    [Range(10, 25)]
    [SerializeField] private float _damagePerShot;

    private Rigidbody2D _rigidbody2D;

    public event Action<Bullet> CollisionHappened;

    public float DamagePerShot => _damagePerShot;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_collisionLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            CollisionHappened?.Invoke(this);
        }
    }

    public void AddForce(Vector2 direction)
    {
        _rigidbody2D.AddForce(direction * _speed, ForceMode2D.Impulse);
    }

    public void RotateToDirection(float rotateValueY)
    {
        float leftRotationY = -180f;
        Quaternion leftRotation = Quaternion.Euler(0, leftRotationY, 0);

        if (rotateValueY == leftRotationY || rotateValueY == -leftRotationY)
            transform.rotation = leftRotation;
        else
            transform.rotation = Quaternion.identity;
    }
}