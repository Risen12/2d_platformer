using System;
using UnityEngine;

public class VisibleZone : MonoBehaviour
{
    public event Action<Vector2> EnemyEntered;
    public event Action EnemyExited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
        {
            EnemyEntered?.Invoke(attacker.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
        {
            EnemyExited?.Invoke();
        }
    }
}
