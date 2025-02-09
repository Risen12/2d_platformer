using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBarViewWithTwoColors : SmoothHealthBarView
{
    [SerializeField] private Slider _tempSlider;
    [SerializeField] private float _tempFillShowTime;

    private Coroutine _changeValueCoroutine;
    private WaitForSeconds _tempShowDelay;

    private void Awake()
    {
        base.Awake();
        _tempShowDelay = new WaitForSeconds(_tempFillShowTime);
    }

    private void Start()
    {
        _tempSlider.maxValue = Slider.maxValue;
        _tempSlider.minValue = Slider.minValue;
        _tempSlider.value = Slider.value;
    }

    protected override void ShowHealth(float value)
    {
        if (_changeValueCoroutine != null)
            StopCoroutine(_changeValueCoroutine);

        _changeValueCoroutine = StartCoroutine(ChangeValue(value));
    }

    private IEnumerator ChangeValue(float targetValue)
    {
        if (Slider.value > targetValue)
        {
            yield return ChangeSliderValue(Slider, targetValue);
            yield return _tempShowDelay;
            yield return ChangeSliderValue(_tempSlider, targetValue);
        }
        else
        {
            yield return ChangeSliderValue(_tempSlider, targetValue);
            yield return _tempShowDelay;
            yield return ChangeSliderValue(Slider, targetValue);
        }
    }

    private IEnumerator ChangeSliderValue(Slider slider, float targetValue)
    {
        while (slider.value != targetValue)
        {
            slider.value = Mathf.MoveTowards(slider.value, targetValue, SmoothIndex * Time.deltaTime);

            yield return null;
        }
    }
}