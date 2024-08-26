using System;
using UnityEngine;

public class CoinSpawne : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private float _rightBorderX;
    [SerializeField] private Vector2 _firstCoinPosition;

    private void Start()
    {
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        float distanceX = Math.Abs(_firstCoinPosition.x);
        float currentCoinPositionX = _firstCoinPosition.x;
        float nextCoinPositionX = currentCoinPositionX + distanceX;

        while (currentCoinPositionX < _rightBorderX)
        {
            Instantiate(_coinPrefab, new Vector3(currentCoinPositionX, _firstCoinPosition.y, 0), Quaternion.identity);
            currentCoinPositionX = nextCoinPositionX;
            nextCoinPositionX += distanceX;
        }
    }
}
