using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private float _leftRotationY;
    private float _currentRotationY;

    public event Action DirectionChanged;

    public float CurrentRotationY => _currentRotationY;
    public float LeftRotationY => _leftRotationY;

    private void Awake()
    {
        _leftRotationY = -180f;
    }

    public void ChangeDirection(float direction)
    {
        Quaternion leftRotation = Quaternion.Euler(0, _leftRotationY, 0);
        DirectionChanged?.Invoke();

        if (direction > 0)
        {
            transform.rotation = Quaternion.identity;
            _currentRotationY = 0f;
        }
        else
        {
            transform.rotation = leftRotation;
            _currentRotationY = _leftRotationY;
        }
    }
}