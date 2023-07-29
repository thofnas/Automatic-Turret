using Turret.StateMachine;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TurretStateMachine _turret;
        
        public TurretStateMachine Turret { get => _turret; }
    }
}
