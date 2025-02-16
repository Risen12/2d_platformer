using System;
using System.Collections;
using UnityEngine;

public class VampirismReloader : MonoBehaviour
{
    [SerializeField] private VampirismIndicator _reloadIndicator;
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private float _reloadTime = 4f;

    private bool _isReady;
    private WaitForSeconds _reloadDelay;
    private Coroutine _reloadCoroutine;

    public bool IsReady => _isReady;

    private void Awake()
    {
        _isReady = true;
        _reloadDelay = new WaitForSeconds(_reloadTime);
    }

    private void OnEnable()
    {
        _vampirism.Activated += OnActivated;
        _vampirism.ActiveTimeEnded += OnActiveTimeEnded;
    }

    private void OnDisable()
    {
        _vampirism.Activated -= OnActivated;
        _vampirism.ActiveTimeEnded -= OnActiveTimeEnded;
    }

    private void OnActivated()
    {
        _isReady = false;
    }

    private void OnActiveTimeEnded()
    { 
        if(_reloadCoroutine != null)
            StopCoroutine(_reloadCoroutine);

        _reloadCoroutine = StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        _reloadIndicator.LaucnhRisingIndicator(0f, _reloadTime);
        yield return _reloadDelay;

        _isReady = true;
    }
}