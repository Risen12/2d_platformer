using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VampirismIndicator : MonoBehaviour
{
    [SerializeField] private VampirismReloader _vampirismReloader;
    [SerializeField] private Vampirism _vampirism;

    private Slider _reloadIndicator;
    private Coroutine _IndicatorCoroutine;

    private void Awake()
    {
        _reloadIndicator = GetComponent<Slider>();
    }

    public void LaucnhRisingIndicator(float minValue, float maxValue)
    {
        SetIndicatorParameters(minValue, maxValue, minValue);

        _IndicatorCoroutine = StartCoroutine(LaunchIndicator(false, maxValue));
    }

    public void LaunchLowingIndicator(float minValue, float maxValue)
    {
        SetIndicatorParameters(minValue, maxValue, maxValue);

        _IndicatorCoroutine = StartCoroutine(LaunchIndicator(true, maxValue));
    }

    private void SetIndicatorParameters(float minValue, float maxValue, float startValue)
    {
        _reloadIndicator.minValue = minValue;
        _reloadIndicator.maxValue = maxValue;
        _reloadIndicator.value = startValue;

        if (_IndicatorCoroutine != null)
            StopCoroutine(_IndicatorCoroutine);
    }

    private IEnumerator LaunchIndicator(bool isReverse, float duration)
    {
        yield return null;

        float targetValue = 0f;

        if (isReverse)
            targetValue = _reloadIndicator.minValue;
        else
            targetValue = _reloadIndicator.maxValue;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float maxDelta = _reloadIndicator.maxValue / duration * Time.deltaTime;

            _reloadIndicator.value = Mathf.MoveTowards(_reloadIndicator.value, targetValue, maxDelta);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _reloadIndicator.value = targetValue;
    }
}