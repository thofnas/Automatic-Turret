using DG.Tweening;
using Events;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item
{
    public class GearSpawner : PoolerBase<Gear>
    {
        [SerializeField] private Gear _gearPrefab;
        [SerializeField, Range(0F, 10F)] private float _minDistance = 1F;
        [SerializeField, Range(0.1F, 10F)] private float _maxDistance = 3F;
        [SerializeField, Range(0.1F, 10F)] private float _durationOfMoving = 2F;

        [SerializeField] private Ease _easeXZ = Ease.Flash;
        [SerializeField] private Ease _easeY = Ease.OutBounce;

        private Vector3 _spawnPosition = Vector3.zero;
        private bool _isDroppingItemsLocked;

        private void Start()
        {
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnEnemyKilled.AddListener(GameEvents_Enemy_OnKilled);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
        }

        private void OnDestroy()
        {
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnStarted);
            GameEvents.OnEnemyKilled.RemoveListener(GameEvents_Enemy_OnKilled);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
        }

        private void DropGears(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Get();
            }
        }
        
        protected override Gear CreateSetup() => Instantiate(_gearPrefab, _spawnPosition, Quaternion.identity);

        protected override void GetSetup(Gear gear) {
            base.GetSetup(gear);

            gear.transform.rotation = Quaternion.identity;
            gear.transform.position = _spawnPosition;
            gear.ResetItemProperties();
            
            gear.SetPool(Pool);
            
            Vector3 targetPosition = Utilities.GetRandomPositionAtDistance(_spawnPosition, _minDistance, _maxDistance);

            if (Utilities.TryGetGroundPoint(targetPosition, out Vector3 groundPoint, GameManager.ENEMY_LAYER))
                targetPosition = groundPoint;
            else
                targetPosition.y = GameManager.Instance.GroundTransform.position.y;

            gear.transform.DOMoveX(targetPosition.x, _durationOfMoving).SetEase(_easeXZ);
            gear.transform.DOMoveZ(targetPosition.z, _durationOfMoving).SetEase(_easeXZ);
            gear.transform.DOMoveY(targetPosition.y, _durationOfMoving).SetEase(_easeY);
            gear.transform.DORotate(Random.rotation.eulerAngles, _durationOfMoving * 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                gear.transform.DORotate(Quaternion.identity.eulerAngles, _durationOfMoving * 0.5f);
            });
        }
        
        private void GameEvents_Wave_OnStarted()
        {
            _isDroppingItemsLocked = false;
            int amountOfGears = WaveManager.Instance.AmountOfGearsInCurrentWave;
            InitPool(_gearPrefab, Mathf.FloorToInt(amountOfGears * 0.5f), amountOfGears);
        }
        
        private void GameEvents_Enemy_OnKilled(Enemy.Enemy enemy)
        {
            if (_isDroppingItemsLocked) return;
            
            _spawnPosition = enemy.transform.position;

            DropGears(enemy.GearsToDrop);
        }
        
        private void GameEvents_Wave_OnLost()
        {
            _isDroppingItemsLocked = true;
            Pool.Clear();
        }
    }
}
