using System;
using UnityEngine;

public class AttackPoint : MonoBehaviour 
{
    public event Action CanAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Attacker attacker))
            CanAttack?.Invoke();
    }
}
