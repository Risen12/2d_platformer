using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Attacker : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private MySceneManager _mySceneManager;
    [SerializeField] private BulletSpawner _bulletSpawner;

    private Mover _mover;
    private Coroutine _animationBeforeShot;
    private WaitForSeconds _delayBeforeShot;
    private WaitForSeconds _delayBeforeDie;
    private float _health;

    public event Action Attacked;
    public event Action DamageTaken;
    public event Action Died;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _health;

    private void OnDisable()
    {
        if (_animationBeforeShot != null)
            StopCoroutine(_animationBeforeShot);
    }

    private void Awake()
    {
        float delay = 0.37f;
        _delayBeforeShot = new WaitForSeconds(delay);

        float _dieDelay = 0.6f;
        _delayBeforeDie = new WaitForSeconds(_dieDelay);

        _mover = GetComponent<Mover>();

        _health = _maxHealth;
    }

    private void Update()
    {
        Shot();
    }

    private void Shot()
    {
        KeyCode attackButton = KeyCode.F;

        if (Input.GetKeyDown(attackButton) && _mover.IsGrounded && _mover.IsMoving == false)
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
        _bulletSpawner.GetBullet();
    }

    private IEnumerator HandleDie()
    {
        Died?.Invoke();
        _mover.Stop(true);

        yield return _delayBeforeDie;

        gameObject.SetActive(false);
        _mySceneManager.ReloadLevel();
    }

    public void TakeDamage(float damage)
    {
        _mover.Stop(true);

        _health -= damage;

        if (_health <= 0)
            StartCoroutine(HandleDie());

        DamageTaken?.Invoke();
    }

    public void UseFirstAidKit(float healthPoints)
    {
        if (_health + healthPoints > _maxHealth)
            _health = _maxHealth;
        else
            _health += healthPoints;
    }
}
