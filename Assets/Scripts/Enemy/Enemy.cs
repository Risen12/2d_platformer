using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D), typeof(EnemyMover))]
public class Enemy : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private EnemyMover _enemyMover;

    private bool _isMoving;
    private WaitForSeconds _delayBeforeDeath;

    public event Action Died;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _enemyMover = GetComponent<EnemyMover>();

        float delay = 1f;
        _delayBeforeDeath = new WaitForSeconds(delay);
        _isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet bullet))
        {
            Die();
        }
    }

    private IEnumerator WaitDelayBeforeDeath()
    {
        yield return _delayBeforeDeath;

        gameObject.SetActive(false);
    }

    private void Die()
    {
        float colliderSizeY = 0.25f;
        _enemyMover.Stop();
        _isMoving = false;

        Died?.Invoke();
        _boxCollider.size = new Vector2(_boxCollider.size.x, colliderSizeY);

        StartCoroutine(WaitDelayBeforeDeath());
    }
}
