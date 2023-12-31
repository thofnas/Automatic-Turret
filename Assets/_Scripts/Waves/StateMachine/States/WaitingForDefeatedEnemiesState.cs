﻿using Events;
using Managers;
using UnityEngine;

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
            //wave ends if all enemies defeated and it was the last subwave
            if (!EnemyManager.Instance.IsAnyEnemyExists() && WaveManager.Instance.IsCurrentSubwaveTheLast())
            {
                GameEvents.OnWaveWon.Invoke();
                SwitchState(Factory.WaitingToFinishWave());
                return;
            }
            
            // start new subwave if they should be
            if (!EnemyManager.Instance.IsAnyEnemyExists() && !WaveManager.Instance.IsCurrentSubwaveTheLast())
                SwitchState(Factory.SpawningEnemies());
        }
        
        private void GameEvents_Turret_OnDestroyed()
        {
            if (!EnemyManager.Instance.IsAnyEnemyExists() && WaveManager.Instance.IsCurrentSubwaveTheLast()) return;
            
            GameEvents.OnWaveLost.Invoke();
            SwitchState(Factory.WaitingToFinishWave());
        }
    }
}
