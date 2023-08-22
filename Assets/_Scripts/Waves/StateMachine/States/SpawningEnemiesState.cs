using System.Collections;
using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;

namespace Waves.StateMachine.States
{
    public class SpawningEnemiesState : WaveState
    {
        public SpawningEnemiesState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        private IEnumerator _spawnEnemiesRoutine;

        public override void EnterState()
        {
            Time.timeScale = 1;
            GameEvents.OnTurretDestroyed.AddListener(GameEvents_Turret_OnDestroyed);
            GameEvents.OnSubWaveStarted.Invoke();
            
            _spawnEnemiesRoutine = SpawnEnemiesRoutine();
            if (Ctx != null) Ctx.StartCoroutine(_spawnEnemiesRoutine);
        }

        public override void ExitState()
        {
            GameEvents.OnTurretDestroyed.RemoveListener(GameEvents_Turret_OnDestroyed);
            if (Ctx != null) Ctx.StopCoroutine(_spawnEnemiesRoutine);
            _spawnEnemiesRoutine = null;
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (Ctx.OnAllEnemiesSpawned)
                SwitchState(Factory.WaitingForDefeatedEnemies());
        }
        
        public IEnumerator SpawnEnemiesRoutine()
        {
            SubWave subWave = WaveManager.Instance.GetCurrentSubWaveData();

            yield return new WaitForSeconds(subWave.SpawnDelay);

            List<Enemy.Enemy> shuffledEnemies = Utilities.ShuffleList(FlattenEnemyData(subWave.EnemiesData));

            foreach (Enemy.Enemy enemy in shuffledEnemies)
            {
                EnemyManager.Instance.SpawnEnemy(enemy, Quaternion.identity);
                
                yield return new WaitForSeconds(subWave.SpawnDelay);
            }
            
            GameEvents.OnAllEnemiesSpawned.Invoke();
        }

        private static IEnumerable<Enemy.Enemy> FlattenEnemyData(List<EnemyData> datas)
        {
            List<Enemy.Enemy> list = new();

            foreach (EnemyData data in datas)
            {
                for (int i = 0; i < data.EnemyQuantity; i++)
                {
                    list.Add(data.EnemyPrefab);
                }
            }

            return list;
        }
        
        private void GameEvents_Turret_OnDestroyed()
        {
            GameEvents.OnWaveLost.Invoke();
            SwitchState(Factory.WaitingToFinishWave());
        }
    }
}
