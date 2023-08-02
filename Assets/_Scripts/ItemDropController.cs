using System;
using Managers;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    [SerializeField] private Transform _itemPrefab;
    [SerializeField, Min(1)] private int _spawnQuantity = 1;
    [SerializeField, Range(0F, 10F)] private float _minDistance = 1F;
    [SerializeField, Range(0.1F, 10F)] private float _maxDistance = 3F;
    [SerializeField, Range(0.1F, 10F)] private float _durationOfMoving = 2F;

    public void DropItems(Func<float, float> easingEquation)
    {
        for (int i = 0; i < _spawnQuantity; i++)
        {
            Transform newItem = Instantiate(_itemPrefab, transform.position, Quaternion.identity);

            Vector3 targetPosition = Utilities.GetRandomPositionAtDistance(transform.position, _minDistance, _maxDistance);
        
            GameManager.Instance.StartCoroutine(Utilities.EaseObjectToPositionRoutine(newItem, transform.position, targetPosition, _durationOfMoving, easingEquation));
        }
    }
    
    public void DropItems(Func<float, float> easingEquationXZ, Func<float, float> easingEquationY)
    {
        for (int i = 0; i < _spawnQuantity; i++)
        {
            Transform newItem = Instantiate(_itemPrefab, transform.position, Quaternion.identity);

            Vector3 targetPosition = Utilities.GetRandomPositionAtDistance(transform.position, _minDistance, _maxDistance);

            GameManager.Instance.StartCoroutine(Utilities.EaseObjectToPositionRoutine(newItem, transform.position, targetPosition, _durationOfMoving,
                easingEquationXZ, easingEquationY));
        }
    }
}
