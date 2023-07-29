using Events;
using UnityEngine;

namespace Waves.StateMachine
{
    public abstract class WaveBaseState
    {
        public WaveBaseState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory)
        {
            Ctx = currentContext;
            Factory = turretStateFactory;
        }
        
        protected readonly WaveStateMachine Ctx;
        protected readonly WaveStateFactory Factory;

        public abstract void EnterState();

        public abstract void ExitState();
        
        public abstract void UpdateState();
        
        public abstract void CheckSwitchStates();
        
        public abstract void InitializeSubState();
        
        public void UpdateStates(){}

        protected void SwitchState(WaveBaseState newState)
        {
            ExitState();
            
            newState.EnterState();

            Ctx.CurrentState = newState;
            
            GameEvents.OnWaveStateChanged.Invoke(Ctx.CurrentState.ToString());
        }
        
        protected void SetSuperState(){}
        
        protected void SetSubState(){}
    }
}
