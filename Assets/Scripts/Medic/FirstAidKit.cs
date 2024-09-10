using UnityEngine;

public class FirstAidKit : MonoBehaviour 
{
    [SerializeField] private float _healthPointsToRestore;

    public float HealthPoints => _healthPointsToRestore;
}
