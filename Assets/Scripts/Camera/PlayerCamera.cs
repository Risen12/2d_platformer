using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private float _leftBorder;
    [SerializeField] private float _rightBorder;
    [SerializeField] private float _topBorder;
    [SerializeField] private float _bottomBorder;

    private float _positionZ;

    private void Awake()
    {
        _positionZ = -10f;
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
