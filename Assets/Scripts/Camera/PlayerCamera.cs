using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private float _leftBorder;
    [SerializeField] private float _rightBorder;
    [SerializeField] private float _topBorder;
    [SerializeField] private float _bottomBorder;
    [SerializeField] private List<Teleport> _teleportControllers;

    private float _positionZ;
    private float _sewerBottomBorder;
    private float _upperBottomBorder;

    private void Awake()
    {
        _sewerBottomBorder = -12f;
        _upperBottomBorder = -1.2f;
        _positionZ = -10f;
    }

    private void OnEnable()
    {
        foreach (Teleport teleporController in _teleportControllers)
        {
            teleporController.Teleported += OnTeleported;
        }
    }

    private void OnDisable()
    {
        foreach (Teleport teleporController in _teleportControllers)
        {
            teleporController.Teleported -= OnTeleported;
        }
    }

    private void OnTeleported(Vector2 newPosition)
    {
        float upperPositon = -3;

        if (newPosition.y == upperPositon)
            _bottomBorder = _upperBottomBorder;
        else
            _bottomBorder = _sewerBottomBorder;
    }

    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = (_mover.transform.position - transform.position).normalized;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + direction.x, _leftBorder, _rightBorder),
            Mathf.Clamp(transform.position.y + direction.y, _bottomBorder, _topBorder),
            _positionZ);
    }
}