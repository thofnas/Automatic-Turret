using DG.Tweening;
using Managers;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private Transform _itemPrefab;
    [SerializeField, Min(1)] private int _spawnQuantity = 1;
    [SerializeField, Range(0F, 10F)] private float _minDistance = 1F;
    [SerializeField, Range(0.1F, 10F)] private float _maxDistance = 3F;
    [SerializeField, Range(0.1F, 10F)] private float _durationOfMoving = 2F;

    public void DropItems(Ease easeXZ, Ease easeY)
    {
        for (int i = 0; i < _spawnQuantity; i++)
        {
            Transform newItem = Instantiate(_itemPrefab, transform.position, Quaternion.identity);

            Vector3 targetPosition = Utilities.GetRandomPositionAtDistance(transform.position, _minDistance, _maxDistance);

            if (Utilities.TryGetGroundPoint(targetPosition, out Vector3 groundPoint, GameManager.ENEMY_LAYER))
                targetPosition = groundPoint;
            else
                targetPosition.y = GameManager.Instance.GroundTransform.position.y;

            newItem.DOMoveX(targetPosition.x, _durationOfMoving).SetEase(easeXZ);
            newItem.DOMoveZ(targetPosition.z, _durationOfMoving).SetEase(easeXZ);
            newItem.DOMoveY(targetPosition.y, _durationOfMoving).SetEase(easeY);
            newItem.DORotate(Random.rotation.eulerAngles, _durationOfMoving / 2, RotateMode.FastBeyond360).OnComplete(() =>
            {
                newItem.DORotate(Quaternion.identity.eulerAngles, _durationOfMoving / 2);
            });
        }
    }
}
