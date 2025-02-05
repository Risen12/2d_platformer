using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    private float _horizontalDirection;
    private float _verticalDirection;
    private KeyCode _runButton;
    private KeyCode _teleportButton;
    private bool _isRunning;

    public event Action TeleportButtonPressed;

    public float VerticalDirection => _verticalDirection;
    public float HorizontalDirection => _horizontalDirection;
    public bool IsRunning => _isRunning;

    private void Awake()
    {
        _teleportButton = KeyCode.T;
        _runButton = KeyCode.LeftShift;
    }

    private void Update()
    {
        _horizontalDirection = Input.GetAxisRaw(HorizontalAxis);
        _verticalDirection = Input.GetAxisRaw(VerticalAxis);

        if (Input.GetKey(_runButton))
        {
            _isRunning = true;
        }
        else
        { 
            _isRunning = false;
        }

        if (Input.GetKeyDown(_teleportButton))
            TeleportButtonPressed?.Invoke();
    }
}