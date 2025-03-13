using System;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Button _teleportButton;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private TeleportOptions _selectedOption;
    [SerializeField] private Fader _fadeController;

    private bool _isPlayerInTeleportZone;
    private Coroutine _teleportCoroutine;

    public event Action<Vector2> Teleported;

    private void Awake()
    {
        _isPlayerInTeleportZone = false;
    }

    private void OnEnable()
    {
        _inputReader.TeleportButtonPressed += OnTeleportButtonPressed;
        _fadeController.FadedIn += OnFadedIn;
    }

    private void OnDisable()
    {
        _inputReader.TeleportButtonPressed -= OnTeleportButtonPressed;
        _fadeController.FadedIn -= OnFadedIn;
    }

    private void OnTeleportButtonPressed()
    {
        if (_isPlayerInTeleportZone == false)
            return;

        if(_teleportCoroutine != null)
            StopCoroutine(_teleportCoroutine);

        _teleportCoroutine = StartCoroutine(_fadeController.FadeInAndOut());
    }

    private void OnFadedIn()
    {
        if (_isPlayerInTeleportZone == false)
            return;

        float upperPositionY = -3f;
        float lowerPositionY = -13f;
        Vector2 upperPosition = new Vector2(transform.position.x, upperPositionY);
        Vector2 lowerPosition = new Vector2(transform.position.x, lowerPositionY);

        if (_selectedOption == TeleportOptions.UpperTeleport)
            Teleported?.Invoke(lowerPosition);
        else
            Teleported?.Invoke(upperPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _))
        {
            _teleportButton.gameObject.SetActive(true);
            _isPlayerInTeleportZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _))
        {
            _teleportButton.gameObject.SetActive(false);
            _isPlayerInTeleportZone = false;
        }
    }
}