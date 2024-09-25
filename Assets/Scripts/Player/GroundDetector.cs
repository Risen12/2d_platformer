using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Transform _groundVerifier;

    private bool _isGrounded;

    public event Action<bool> GroundStateChanged;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _isGrounded = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        VerifyGroundedState();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        VerifyGroundedState();
    }

    private bool VerifyGroundedState()
    {
        float colliderSizeX = 0.4f;
        float colliderSizeY = 0.1f;

        _isGrounded = Physics2D.OverlapCapsule(_groundVerifier.position, new Vector2(colliderSizeX, colliderSizeY), CapsuleDirection2D.Horizontal, 0f, _groundLayer) ||
                      Physics2D.OverlapCapsule(_groundVerifier.position, new Vector2(colliderSizeX, colliderSizeY), CapsuleDirection2D.Horizontal, 0f, _platformLayer);

        GroundStateChanged?.Invoke(_isGrounded);

        return _isGrounded;
    }
}
