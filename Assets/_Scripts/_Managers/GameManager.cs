using _Events;
using Turret.StateMachine;
using UnityEngine;

namespace _Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TurretStateMachine _turret;
        
        public int TurretHealth { get; }
        public TurretStateMachine Turret { get => _turret; }
        
        private int _turretHealth;
        
        private void OnEnable()
        {
            
        }
    }
}
