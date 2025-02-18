using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Vampirism))]
public class VampirismAnimator : MonoBehaviour
{
    private readonly int ActiveTimeEndedTrigger = Animator.StringToHash("ActiveTimeEnded");

    [SerializeField] private VampirismReloader _vampirismReloader;

    private Animator _animator;
    private Vampirism _vampirism;

    public event Action EntryAnimationEnded;
    public event Action EndAnimationEnded;

    private void Awake()
    {
        _vampirism = GetComponent<Vampirism>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() => 
        _vampirism.ActiveTimeEnded += OnActiveTimeEnded;

    private void OnDisable() => 
        _vampirism.ActiveTimeEnded -= OnActiveTimeEnded;

    private void OnActiveTimeEnded() => 
        _animator.SetTrigger(ActiveTimeEndedTrigger);

    private void OnEntryAnimationEnded() => 
        EntryAnimationEnded?.Invoke();

    private void OnEndAnimationEnded() => 
        EndAnimationEnded?.Invoke();
}