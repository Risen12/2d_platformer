using System;
using UnityEngine;
using UnityEngine.UI;

public class TeleporController : MonoBehaviour
{
    [SerializeField] private Button _teleportButton;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Options _selectedOption;
    [SerializeField] private FadeController _fadeController;

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

        StartCoroutine(_fadeController.FadeInAndOut());
    }

    private void OnFadedIn()
    {
        if (_isPlayerInTeleportZone == false)
            return;

        float upperPositionY = -3f;
        float lowerPositionY = -13f;
        Vector2 upperPosition = new Vector2(transform.position.x, upperPositionY);
        Vector2 lowerPosition = new Vector2(transform.position.x, lowerPositionY);

        if (_selectedOption == Options.UpperTeleport)
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

    private enum Options { UpperTeleport, DownTeleport }
}