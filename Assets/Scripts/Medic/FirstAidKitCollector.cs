using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class FirstAidKitCollector : MonoBehaviour
{
    private Attacker _attacker;

    private void Awake()
    {
        _attacker = GetComponent<Attacker>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FirstAidKit kit))
        {
            if (NeedUseFirstAidKit())
            {
                _attacker.UseFirstAidKit(kit.HealthPoints);
                Destroy(kit.gameObject);
            }
        }
    }

    private bool NeedUseFirstAidKit()
    {
        if (_attacker.CurrentHealth < _attacker.MaxHealth)
            return true;
        else
            return false;
    }
}
