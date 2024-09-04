using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VisibleZone : MonoBehaviour
{
    public event Action<Vector2> EnemyEntered;
    public event Action EnemyExited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            EnemyEntered?.Invoke(direction);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
        {
            EnemyExited?.Invoke();
        }
    }

    public void Rotate(bool left)
    {
        float leftY = 180f;
        float rightY = 0f;

        if (left)
            transform.Rotate(new Vector3(0, leftY, 0));
        else
            transform.Rotate(new Vector3(0, rightY, 0));
    }
}
