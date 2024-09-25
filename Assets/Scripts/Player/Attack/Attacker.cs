using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(GroundDetector))]
public class Attacker : MonoBehaviour
{
    [SerializeField] private BulletSpawner _bulletSpawner;
    
    private GroundDetector _groundDetector;
    private Mover _mover;
    private Coroutine _animationBeforeShot;
    private WaitForSeconds _delayBeforeShot;

    public event Action Attacked;

    private void Awake()
    {
        float delay = 0.37f;
        _delayBeforeShot = new WaitForSeconds(delay);

        _mover = GetComponent<Mover>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void OnDisable()
    {
        if (_animationBeforeShot != null)
            StopCoroutine(_animationBeforeShot);
    }

    private void Update()
    {
        Shot();
    }

    private void Shot()
    {
        KeyCode attackButton = KeyCode.F;

        if (Input.GetKeyDown(attackButton) && _groundDetector.IsGrounded && _mover.IsMoving == false)
        {
            Attacked?.Invoke();

            if(_animationBeforeShot != null)
                StopCoroutine( _animationBeforeShot);

            _animationBeforeShot = StartCoroutine(ShotAfterAnimation());
        }
    }

    private IEnumerator ShotAfterAnimation()
    {
        yield return _delayBeforeShot;
        _bulletSpawner.PrepareBullet();
    }
}
