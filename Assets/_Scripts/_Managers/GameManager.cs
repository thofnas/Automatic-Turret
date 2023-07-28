using _Events;
using Turret.StateMachine;
using UnityEngine;

namespace _Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TurretStateMachine _turret;
        
        public TurretStateMachine Turret { get => _turret; }
    }
}
