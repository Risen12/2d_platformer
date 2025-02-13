using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VampirismAnimatorController : MonoBehaviour
{
    private readonly int ActiveTimeEndedTrigger = Animator.StringToHash("ActiveTimeEnded");

    [SerializeField] private VampirismReloader _vampirismReloader;

    private Animator _animator;

    public event Action EntryAnimationEnded;
    public event Action EndAnimationEnded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() => _vampirismReloader.ActiveTimeEnded += OnActiveTimeEnded;

    private void OnDisable() => _vampirismReloader.ActiveTimeEnded -= OnActiveTimeEnded;

    private void OnActiveTimeEnded() => _animator.SetTrigger(ActiveTimeEndedTrigger);

    private void OnEntryAnimationEnded() => EntryAnimationEnded?.Invoke();

    private void OnEndAnimationEnded() => EndAnimationEnded?.Invoke();
}