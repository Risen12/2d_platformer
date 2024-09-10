using UnityEngine;
using System.Collections.Generic;

public class FirstAidKitSpawner : MonoBehaviour
{
    [SerializeField] private int _firstAidKitsCount;
    [SerializeField] private FirstAidKit _firstAidKitPrefab;
    [SerializeField] private List<Transform> _firstAidKitPositions;

    private List<Vector2> _blockedPositions;

    private void Start()
    {
        _blockedPositions = new List<Vector2>();
        GenerateFirstAidKits();
    }

    private void GenerateFirstAidKit()
    { 
        
    }

    private void GenerateFirstAidKits()
    {
        for (int i = 0; i < _firstAidKitsCount; i++)
        {
            Vector2 position = GetPosition();
            Instantiate(_firstAidKitPrefab, position, Quaternion.identity);
        }
    }

    private bool ValidatePosition(Vector2 position)
    { 
        if(_blockedPositions.Contains(position))
            return false;
        else
            return true;
    }

    private Vector2 GetPosition()
    {
        Vector2 position = new Vector2();
        bool isPositionValidate;
        Transform nextPosition;

        do
        {
            nextPosition = _firstAidKitPositions[Random.Range(0, _firstAidKitPositions.Count)];
            isPositionValidate = ValidatePosition(position);
        } while (isPositionValidate == false);

        position = nextPosition.position;

        return position;
    }
}
