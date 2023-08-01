using Turret.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;
using Waves.StateMachine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TurretStateMachine _turretStateMachine;
        [SerializeField] private WaveStateMachine _waveStateMachine;
        
        public TurretStateMachine TurretStateMachine { get => _turretStateMachine; }
        public WaveStateMachine WaveStateMachine { get => _waveStateMachine; }
    }
}
