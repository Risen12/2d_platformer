using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private const string GroundLayerName = "Ground";

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
        if (collision.gameObject.TryGetComponent(out Enemy enemy) || collision.gameObject.layer == LayerMask.NameToLayer(GroundLayerName))
        {
            CollisionHappened?.Invoke(this);
        }
    }

    public void AddForce(Vector2 direction)
    {
        _rigidbody2D.AddForce(direction * _speed, ForceMode2D.Impulse);
    }

    public void SetDirection(Vector2 direction)
    {
        float leftRotationY = -180f;
        Quaternion leftRotation = Quaternion.Euler(0, leftRotationY, 0);

        if (direction == Vector2.left)
            transform.rotation = leftRotation;
        else
            transform.rotation = Quaternion.identity;
    }
}
