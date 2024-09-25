using UnityEngine;

[RequireComponent(typeof(Health))]
public class FirstAidKitCollector : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FirstAidKit kit))
        {
            if (NeedUseFirstAidKit())
            {
                _health.UseFirstAidKit(kit.HealthPoints);
                Destroy(kit.gameObject);
            }
        }
    }

    private bool NeedUseFirstAidKit()
    {
        if (_health.CurrentHealth < _health.MaxHealth)
            return true;
        else
            return false;
    }
}
