using System;
using Events;
using Turret.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;
using Waves.StateMachine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public const int ENEMY_LAYER = 7;
        public const int GROUND_LAYER = 8;
        
        public int TotalGearCount { get; private set; }

        [SerializeField] private TurretStateMachine _turretStateMachine;
        [SerializeField] private WaveStateMachine _waveStateMachine;
        [SerializeField] private Transform _groundTransform;
        
        public TurretStateMachine TurretStateMachine { get => _turretStateMachine; }
        public WaveStateMachine WaveStateMachine { get => _waveStateMachine; }
        public Transform GroundTransform { get => _groundTransform; }

        public void Initialize()
        {
            GameEvents.OnItemPicked.AddListener(GameEvents_Item_OnItemPicked);
        }

        private void OnDestroy()
        {
            GameEvents.OnItemPicked.RemoveListener(GameEvents_Item_OnItemPicked);
        }
        
        private void GameEvents_Item_OnItemPicked()
        {
            TotalGearCount++;
            GameEvents.OnGearAmountChanged.Invoke();
        }
        
        
    }
}
