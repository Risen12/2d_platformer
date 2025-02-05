using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBarView : HealthBarView
{
    [SerializeField] private float _smoothIndex;
    private Coroutine _changeValueCoroutine;

    protected override void ShowHealth(float value)
    {
        if (_changeValueCoroutine != null)
            StopCoroutine(_changeValueCoroutine);

        _changeValueCoroutine = StartCoroutine(ChangeValue(value));
    }

    private IEnumerator ChangeValue(float targetValue)
    {
        while (Slider.value != targetValue)
        {
            Slider.value = Mathf.MoveTowards(Slider.value, targetValue, _smoothIndex);

            yield return null;
        }
    }
}