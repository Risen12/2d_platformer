using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class VampirismActivator : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private VampirismReloader _reloader;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _inputReader.VamprisimButtonPressed += OnVampirisimButtonPressed;
        _vampirism.HealthTaken += OnHealthTaken;
        _vampirism.VisibleTimeEnded += OnVisibleTimeEnded;
    }

    private void OnDisable()
    {
        _inputReader.VamprisimButtonPressed -= OnVampirisimButtonPressed;
        _vampirism.HealthTaken -= OnHealthTaken;
        _vampirism.VisibleTimeEnded -= OnVisibleTimeEnded;
    }

    private void OnVampirisimButtonPressed()
    {
        if (_reloader.IsReady && _vampirism.gameObject.activeSelf == false)
        {
            _vampirism.gameObject.SetActive(true);
            _vampirism.Activate();
        }
    }

    private void OnHealthTaken(float value)
    {
        _health.Restore(value);
    }

    private void OnVisibleTimeEnded()
    {
        _vampirism.gameObject.SetActive(false);
    }
}