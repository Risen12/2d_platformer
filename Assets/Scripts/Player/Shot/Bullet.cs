using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    private const string GroundLayerName = "Ground";
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    public event Action<Bullet> CollisionHappened;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void SetDirection(bool isLeft)
    { 
        if(isLeft)
            _spriteRenderer.flipX = true;
    }
}
