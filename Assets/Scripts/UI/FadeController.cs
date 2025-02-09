using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _delayDuration;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Color _fadeInColor;
    [SerializeField] private Color _fadeOutColor;

    private WaitForSeconds _delay;
    private bool _isPlaying;

    public event Action FadedIn;

    private void Awake()
    {
        _isPlaying = false;
        _delay = new WaitForSeconds(_delayDuration);
    }

    public IEnumerator FadeInAndOut()
    {
        if (_isPlaying == false)
            _isPlaying = true;
        else
            yield break;

        yield return ChangeFade(_fadeInColor);
        FadedIn?.Invoke();
        yield return ChangeFadeWithStartDelay(_fadeOutColor);

        _isPlaying = false;
    }

    private IEnumerator ChangeFade(Color targetColor)
    {
        float elapsedTime = 0f;
        Color initialColor = _fadeImage.color;

        while (elapsedTime < _fadeDuration)
        {
            _fadeImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _fadeImage.color = targetColor;
    }

    private IEnumerator ChangeFadeWithStartDelay(Color targetColor)
    {
        yield return _delay;

        yield return ChangeFade(targetColor);
    }
}