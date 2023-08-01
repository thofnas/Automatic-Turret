using Events;
using Managers;
using UnityEngine;

namespace Waves.StateMachine.States
{
    public class WaitingForDefeatedEnemiesState : WaveBaseState
    {
        public WaitingForDefeatedEnemiesState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        public override void EnterState() { }

        public override void ExitState() => GameEvents.OnSubWaveEnded.Invoke();

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            //wave ends if all enemies defeated and it was the last subwave
            if (!EnemyManager.Instance.IsAnyEnemyExists() && Ctx.CurrentSubWaveID >= Ctx.CurrentSubWaveIDMax)
                SwitchState(Factory.WaitingToStartWave());

            // start new subwave if they should be
            if (!EnemyManager.Instance.IsAnyEnemyExists() && Ctx.CurrentSubWaveID < Ctx.CurrentSubWaveIDMax)
                SwitchState(Factory.SpawningEnemies());
        }
    }
}
