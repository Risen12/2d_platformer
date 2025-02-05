using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class EnemyRotationController : MonoBehaviour
{
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private EnemyMover _enemyMover;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _enemyMover.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        _enemyMover.DirectionChanged -= OnDirectionChanged;
    }

    private void OnDirectionChanged()
    {
        _rectTransform.rotation = Quaternion.Euler(_targetRotation);
    }
}