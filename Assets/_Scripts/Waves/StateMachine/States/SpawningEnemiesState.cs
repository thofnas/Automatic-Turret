using System.Collections;
using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;

namespace Waves.StateMachine.States
{
    public class SpawningEnemiesState : WaveBaseState
    {
        public SpawningEnemiesState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        private IEnumerator _spawnEnemiesRoutine;

        public override void EnterState()
        {
            GameEvents.OnSubWaveStarted.Invoke();
            
            _spawnEnemiesRoutine = SpawnEnemiesRoutine();
            
            Ctx.StartCoroutine(_spawnEnemiesRoutine);
        }

        public override void ExitState() => Ctx.StopCoroutine(_spawnEnemiesRoutine);

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (Ctx.OnAllEnemiesSpawned)
                SwitchState(Factory.WaitingForDefeatedEnemies());
        }
        
        public IEnumerator SpawnEnemiesRoutine()
        {
            SubWave subWave = Ctx.GetCurrentSubWaveData();
            yield return new WaitForSeconds(subWave.SpawnDelay);

            List<Enemy> shuffledEnemies = Utilities.ShuffleList(FlattenEnemyData(subWave.EnemiesData));
            
            foreach (Enemy enemy in shuffledEnemies)
            {
                Vector3 randomSpawnPosition = Utilities.GetRandomPositionAtDistance(
                    GameManager.Instance.TurretStateMachine.GetTransform().position,
                    EnemyManager.MAX_DISTANCE_TO_SPAWN_ENEMY,
                    EnemyManager.MIN_DISTANCE_TO_SPAWN_ENEMY);
                
                EnemyManager.Instance.SpawnEnemy(enemy, randomSpawnPosition, Quaternion.identity);
                
                yield return new WaitForSeconds(subWave.SpawnDelay);
            }
            
            GameEvents.OnAllEnemiesSpawned.Invoke();
        }

        private static IEnumerable<Enemy> FlattenEnemyData(List<EnemyData> datas)
        {
            List<Enemy> list = new();

            foreach (EnemyData data in datas)
            {
                for (int i = 0; i < data.EnemyQuantity; i++)
                {
                    list.Add(data.EnemyPrefab);
                }
            }

            return list;
        }
    }
}
