using System;
using UnityEngine;

public class AttackPoint : MonoBehaviour 
{
    public event Action<bool> AttackStateChanged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
            AttackStateChanged?.Invoke(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Attacker attacker))
            AttackStateChanged?.Invoke(false);
    }
}
