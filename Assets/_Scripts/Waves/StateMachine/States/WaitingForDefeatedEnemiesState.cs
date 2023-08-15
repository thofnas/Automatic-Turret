﻿using Events;
using Managers;

namespace Waves.StateMachine.States
{
    public class WaitingForDefeatedEnemiesState : WaveState
    {
        public WaitingForDefeatedEnemiesState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }

        public override void EnterState()
        {
            GameEvents.OnTurretDestroyed.AddListener(GameEvents_Turret_OnDestroyed);
        }

        public override void ExitState()
        {
            GameEvents.OnTurretDestroyed.RemoveListener(GameEvents_Turret_OnDestroyed);
            GameEvents.OnSubWaveEnded.Invoke();
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (GameManager.Instance.TurretStateMachine.IsDestroyed) return;
            
            //wave ends if all enemies defeated and it was the last subwave
            if (!EnemyManager.Instance.IsAnyEnemyExists() && WaveManager.Instance.CurrentSubWaveID >= WaveManager.Instance.CurrentSubWaveIDMax)
            {
                GameEvents.OnWaveWon.Invoke();
                SwitchState(Factory.WaitingToFinishWave());
            }

            // start new subwave if they should be
            if (!EnemyManager.Instance.IsAnyEnemyExists() && WaveManager.Instance.CurrentSubWaveID < WaveManager.Instance.CurrentSubWaveIDMax)
                SwitchState(Factory.SpawningEnemies());
        }
        
        private void GameEvents_Turret_OnDestroyed()
        {
            GameEvents.OnWaveLost.Invoke();
            SwitchState(Factory.WaitingToFinishWave());
        }
    }
}
