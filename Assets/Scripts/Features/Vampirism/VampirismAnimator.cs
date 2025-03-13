using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Vampirism))]
public class VampirismAnimator : MonoBehaviour
{
    private readonly int ActiveTimeEndedTrigger = Animator.StringToHash("ActiveTimeEnded");
    private readonly int ActiveTimeStartedTrigger = Animator.StringToHash("ActiveTimeStarted");

    private Animator _animator;
    private Vampirism _vampirism;

    public event Action EndAnimationEnded;

    private void Awake()
    {
        _vampirism = GetComponent<Vampirism>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _vampirism.ActiveTimeEnded += OnActiveTimeEnded;
        _vampirism.ActiveTimeStarted += OnActiveTimeStarted;
    } 

    private void OnDisable()
    {
        _vampirism.ActiveTimeStarted -= OnActiveTimeStarted;
        _vampirism.ActiveTimeEnded -= OnActiveTimeEnded;
    } 

    private void OnActiveTimeEnded() => _animator.SetTrigger(ActiveTimeEndedTrigger);

    private void OnActiveTimeStarted() => _animator.SetTrigger(ActiveTimeStartedTrigger);

    private void EndTimeEnded() => EndAnimationEnded?.Invoke();
}