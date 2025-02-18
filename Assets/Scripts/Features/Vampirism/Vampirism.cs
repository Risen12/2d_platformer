using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VampirismAnimator))]
public class Vampirism : MonoBehaviour
{
    [SerializeField] private float _delayDuration;
    [SerializeField] private VampirismIndicator _indicator;
    [SerializeField] private float _takingHealthPerTime;
    [SerializeField] private float _activeTime = 6f;
    [SerializeField] private LayerMask _enemyLayerMask;

    private bool _isEntryAnimationEnded;
    private VampirismAnimator _animatorController;
    private bool _isEndAnimationEnded;
    private WaitForSeconds _delay;
    private WaitUntil _waitUntilEntryAnimationEnded;
    private WaitUntil _waitUntilEndAnimationEnded;
    private Coroutine _phaseCoroutine;
    private Coroutine _vampireCoroutine;

    public event Action<float> HealthTaken;
    public event Action ActiveTimeEnded;
    public event Action Activated;
    public event Action VisibleTimeEnded;

    private void Awake()
    {
        _animatorController = GetComponent<VampirismAnimator>();
        _delay = new WaitForSeconds(_delayDuration);
        _waitUntilEntryAnimationEnded = new WaitUntil(() => _isEntryAnimationEnded == true);
        _waitUntilEndAnimationEnded = new WaitUntil(() => _isEndAnimationEnded == true);
    }

    private void OnEnable()
    {
        _isEndAnimationEnded = false;
        _isEntryAnimationEnded = false;
        _animatorController.EntryAnimationEnded += OnEntryAnimationEnded;
        _animatorController.EndAnimationEnded += OnEndAnimationEnded;
    }

    private void OnDisable()
    {
        _animatorController.EntryAnimationEnded -= OnEntryAnimationEnded;
        _animatorController.EndAnimationEnded += OnEndAnimationEnded;
    }

    public void Activate()
    {
        if (_phaseCoroutine != null)
            StopCoroutine(_phaseCoroutine);

        Activated?.Invoke();
        _phaseCoroutine = StartCoroutine(StartActivePhase());
        _vampireCoroutine = StartCoroutine(UseAbility());
    }

    private void OnEntryAnimationEnded() => 
        _isEntryAnimationEnded = true;

    private void OnEndAnimationEnded() => 
        _isEndAnimationEnded = true;

    private IEnumerator StartActivePhase()
    {
        yield return _waitUntilEntryAnimationEnded;

        _indicator.LaunchLowingIndicator(0f, _activeTime);

        float time = 0f;

        while (time < _activeTime)
        {
            time += Time.deltaTime;

            yield return null;
        }

        ActiveTimeEnded?.Invoke();
        StopCoroutine(_vampireCoroutine);
        yield return _waitUntilEndAnimationEnded;
        VisibleTimeEnded?.Invoke();
    }

    private Collider2D[] ScanForEnemies()
    {
        float radius = 2f;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayerMask);

        return targets;
    }

    private Health DefineNearestEnemy(Collider2D[] targets)
    {
        Health nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;

                if(target.gameObject.TryGetComponent(out Health health))
                    nearestTarget = health;
            }
        }

        return nearestTarget;
    }

    private IEnumerator UseAbility()
    {
        while (gameObject.activeSelf)
        {
            Collider2D[] targets = ScanForEnemies();

            if (targets.Length > 0)
            { 
                Health target = DefineNearestEnemy(targets);
                target.TakeDamage(_takingHealthPerTime);
                HealthTaken?.Invoke(_takingHealthPerTime);
            }

            yield return _delay;
        }
    }
}