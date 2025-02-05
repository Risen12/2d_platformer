using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RotationController : MonoBehaviour
{
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private Mover _mover;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _mover.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        _mover.DirectionChanged -= OnDirectionChanged;
    }

    private void OnDirectionChanged()
    {
        _rectTransform.rotation = Quaternion.Euler(_targetRotation);
    }
}