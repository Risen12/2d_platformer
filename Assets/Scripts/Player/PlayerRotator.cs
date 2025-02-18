using System;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private float _leftRotationY;

    public event Action DirectionChanged;

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
        }
        else
        {
            transform.rotation = leftRotation;
        }
    }
}