using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VampirismReloader : MonoBehaviour
{
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private VampirismAnimatorController _animatorController;
    [SerializeField] private Slider _reloadIndicator;
    [SerializeField] private float _activeTime = 6f;
    [SerializeField] private float _reloadTime = 4f;

    private bool _isReady;
    private bool _isEntryAnimationEnded;
    private bool _isEndAnimationEnded;
    private Coroutine _phaseCoroutine;
    private WaitUntil _waitUntilEntryAnimationEnded;
    private WaitUntil _waitUntilEndAnimationEnded;

    public event Action ActiveTimeEnded;
    public event Action ReloadTimeEnded;

    public bool IsReady => _isReady;

    private void Awake()
    {
        _isReady = true;
        _isEntryAnimationEnded = false;
        _isEndAnimationEnded = false;

        _waitUntilEndAnimationEnded = new WaitUntil(() => _isEndAnimationEnded == true);
        _waitUntilEntryAnimationEnded = new WaitUntil(() => _isEntryAnimationEnded == true);

        ResetReloadIndicator(_activeTime);
    }

    private void OnEnable()
    {
        _animatorController.EntryAnimationEnded += OnEntryAnimationEnded;
        _animatorController.EndAnimationEnded += OnEndAnimationEnded;
    }

    private void OnDisable()
    {
        _animatorController.EntryAnimationEnded -= OnEntryAnimationEnded;
        _animatorController.EndAnimationEnded -= OnEndAnimationEnded;
    }

    private void OnEntryAnimationEnded() => _isEntryAnimationEnded = true;

    private void OnEndAnimationEnded() => _isEndAnimationEnded = true;

    public void Activate()
    {
        if (_phaseCoroutine != null)
            StopCoroutine(_phaseCoroutine);

        _isEntryAnimationEnded = false;
        _isEndAnimationEnded = false;

        _phaseCoroutine = StartCoroutine(StartPhase());

        _isReady = false;
    }

    private IEnumerator StartPhase()
    {
        _vampirism.gameObject.SetActive(true);

        yield return _waitUntilEntryAnimationEnded;

        float time = 0f;

        while (time < _activeTime)
        {
            time += Time.deltaTime;
            _reloadIndicator.value = time;
            yield return null;
        }

        _reloadIndicator.value = _activeTime;

        ActiveTimeEnded?.Invoke();
        yield return Reload();
    }

    private IEnumerator Reload()
    {
        ResetReloadIndicator(_reloadTime);

        yield return _waitUntilEndAnimationEnded;

        _vampirism.gameObject.SetActive(false);
        float time = 0f;

        while (time < _reloadTime)
        {
            time += Time.deltaTime;
            _reloadIndicator.value = time;
            yield return null;
        }

        _isReady = true;
    }

    private void ResetReloadIndicator(float maxValue)
    {
        _reloadIndicator.value = 0f;
        _reloadIndicator.maxValue = maxValue;
    }
}