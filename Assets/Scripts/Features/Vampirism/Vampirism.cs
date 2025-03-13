using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Vampirism : MonoBehaviour
{
    [SerializeField] private float _reloadTime = 4f;
    [SerializeField] private float _takingHealthPerTime;
    [SerializeField] private float _activeTime = 6f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _vampirismDelay;
    [SerializeField] private VampirismIndicator _vampirismIndicator;
    [SerializeField] private Health _playerHealth;

    private bool _isReady;
    private bool _isEndAnimationEnded;
    private bool _isButtonPressed;
    private SpriteRenderer _spriteRenderer;
    private WaitUntil _waitButtonPressed;
    private WaitUntil _waitEndAnimationEnded;
    private WaitForSeconds _waitReload;
    private VampirismAnimator _vampirismAnimator;

    public event Action ActiveTimeEnded;
    public event Action ActiveTimeStarted;

    private void Awake()
    {
        _isButtonPressed = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _isReady = true;
        _isEndAnimationEnded = false;

        _waitButtonPressed = new WaitUntil(() => _isButtonPressed == true);
        _waitEndAnimationEnded = new WaitUntil(() => _isEndAnimationEnded == true);
        _waitReload = new WaitForSeconds(_reloadTime);
        _vampirismAnimator = GetComponent<VampirismAnimator>();
    }

    private void Start() => StartCoroutine(Launch());

    private void OnEnable() => _vampirismAnimator.EndAnimationEnded += OnEndAnimationEnded;

    private void OnDisable() =>_vampirismAnimator.EndAnimationEnded -= OnEndAnimationEnded;

    public void Activate()
    {
        if (_isReady == false)
            return;

        _isButtonPressed = true;
    }

    private void OnEndAnimationEnded() => _isEndAnimationEnded = true;

    private IEnumerator Launch()
    {
        while (gameObject.activeSelf)
        {
            yield return _waitButtonPressed;

            ActiveTimeStarted?.Invoke();
            _isReady = false;
            _isButtonPressed = false;
            _spriteRenderer.enabled = true;
            _vampirismIndicator.LaunchLowingIndicator(0f, _activeTime);

            float time = 0f;
            float tempTime = 0f;

            while (time < _activeTime)
            {
                time += Time.deltaTime;
                tempTime += Time.deltaTime;

                if (tempTime >= _vampirismDelay)
                {
                    ScanEnemies();
                    tempTime = 0f;
                }

                yield return null;
            }

            ActiveTimeEnded?.Invoke();
            _vampirismIndicator.LaucnhRisingIndicator(0, _reloadTime);
            yield return _waitEndAnimationEnded;
            _spriteRenderer.enabled = false;
            yield return _waitReload;
            _isReady = true;
            _isEndAnimationEnded = false;
        }
    }

    private void ScanEnemies()
    {
        float radius = 2.5f;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayerMask);

        if (targets.Length > 0)
        {
            Health enemy = GetClosestEnemy(targets);
            TakeHealthFromEnemy(enemy);
        }
    }

    private Health GetClosestEnemy(Collider2D[] targets)
    {
        Health nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D target in targets)
        {
            float distance = (target.transform.position - transform.position).sqrMagnitude;

            if (distance < nearestDistance * nearestDistance)
            {
                nearestDistance = distance;

                if (target.gameObject.TryGetComponent(out Health health))
                    nearestTarget = health;
            }
        }

        return nearestTarget;
    }

    private void TakeHealthFromEnemy(Health enemy)
    {
        if (enemy.CurrentHealth < _takingHealthPerTime)
        {
            _playerHealth.Restore(enemy.CurrentHealth);
            enemy.TakeDamage(enemy.CurrentHealth);
        }
        else
        {
            enemy.TakeDamage(_takingHealthPerTime);
            _playerHealth.Restore(_takingHealthPerTime);
        }
    }
}