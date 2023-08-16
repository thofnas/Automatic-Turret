using DG.Tweening;
using Events;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item
{
    public class ItemSpawner : PoolerBase<Item>
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField, Range(0F, 10F)] private float _minDistance = 1F;
        [SerializeField, Range(0.1F, 10F)] private float _maxDistance = 3F;
        [SerializeField, Range(0.1F, 10F)] private float _durationOfMoving = 2F;

        [SerializeField] private Ease _easeXZ = Ease.Flash;
        [SerializeField] private Ease _easeY = Ease.OutBounce;

        private Vector3 _spawnPosition = Vector3.zero;

        private void Start()
        {
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnDestroyed);
        }

        private void OnDestroy()
        {
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnStarted);
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnDestroyed);
        }

        private void DropItems(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Get();
            }
        }
        
        protected override Item CreateSetup() => Instantiate(_itemPrefab, _spawnPosition, Quaternion.identity);

        protected override void GetSetup(Item item) {
            base.GetSetup(item);

            item.transform.position = _spawnPosition;
            item.ResetItemProperties();
            
            item.SetPool(Pool);
            
            Vector3 targetPosition = Utilities.GetRandomPositionAtDistance(_spawnPosition, _minDistance, _maxDistance);

            if (Utilities.TryGetGroundPoint(targetPosition, out Vector3 groundPoint, GameManager.ENEMY_LAYER))
                targetPosition = groundPoint;
            else
                targetPosition.y = GameManager.Instance.GroundTransform.position.y;

            item.transform.DOMoveX(targetPosition.x, _durationOfMoving).SetEase(_easeXZ);
            item.transform.DOMoveZ(targetPosition.z, _durationOfMoving).SetEase(_easeXZ);
            item.transform.DOMoveY(targetPosition.y, _durationOfMoving).SetEase(_easeY);
            item.transform.DORotate(Random.rotation.eulerAngles, _durationOfMoving * 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                item.transform.DORotate(Quaternion.identity.eulerAngles, _durationOfMoving * 0.5f);
            });
        }

        protected override void ReleaseSetup(Item item)
        {
            base.ReleaseSetup(item);
            item.transform.rotation = Quaternion.identity;
        }
        
        private void GameEvents_Wave_OnStarted()
        {
            int amountOfGears = WaveManager.Instance.AmountOfGearsInCurrentWave;
            print(amountOfGears);
            
            InitPool(_itemPrefab, Mathf.FloorToInt(amountOfGears * 0.5f), amountOfGears);
        }
        
        private void GameEvents_Enemy_OnDestroyed(Enemy.Enemy enemy)
        {
            _spawnPosition = enemy.transform.position;

            DropItems(enemy.GearsToDrop);
        }
    }
}
