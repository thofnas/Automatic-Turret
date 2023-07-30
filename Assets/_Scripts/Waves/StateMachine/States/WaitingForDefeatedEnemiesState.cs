using Events;
using Managers;
using UnityEngine;

namespace Waves.StateMachine.States
{
    public class WaitingForDefeatedEnemiesState : WaveBaseState
    {
        public WaitingForDefeatedEnemiesState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        public override void EnterState() { }

        public override void ExitState()
        {
            GameEvents.OnWaveEnded.Invoke();
            Debug.Log(EnemyManager.Instance.HasEnemyInSight());
        }
        
        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (!EnemyManager.Instance.IsAnyEnemyExists())
                SwitchState(Factory.WaitingForPlayer());
        }
    }
}
