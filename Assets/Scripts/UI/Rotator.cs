using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private PlayerRotator _playerRotator;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _playerRotator.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        _playerRotator.DirectionChanged -= OnDirectionChanged;
    }

    private void OnDirectionChanged()
    {
        _rectTransform.rotation = Quaternion.Euler(_targetRotation);
    }
}