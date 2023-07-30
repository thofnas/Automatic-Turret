using System.Collections;
using Managers;
using UnityEngine;

namespace Waves.StateMachine.States
{
    public class SpawningEnemiesState : WaveBaseState
    {
        public SpawningEnemiesState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }


        private IEnumerator _spawnEnemiesRoutine;
        private Enemy _prefab;
        private float _spawnDelay;

        public override void EnterState()
        {
            _spawnDelay = Ctx.GetWaves()[0].SpawnDelay;
            _prefab = Ctx.GetWaves()[0].EnemiesData[0].EnemyPrefab;

            var spawnPosition = new Vector3(Ctx.EnemySpawnPoint.position.x,
                Ctx.EnemySpawnPoint.position.y + _prefab.transform.localScale.y / 2, Ctx.EnemySpawnPoint.position.z);

            _spawnEnemiesRoutine =
                EnemyManager.SpawnEnemiesRoutine(spawnPosition, _prefab, _spawnDelay, Ctx.EnemiesToSpawnCount);
            
            Ctx.StartCoroutine(_spawnEnemiesRoutine);
        }

        public override void ExitState() => Ctx.StopCoroutine(_spawnEnemiesRoutine);

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void CheckSwitchStates()
        {
            if (Ctx.EnemiesToSpawnCount <= 0) 
                SwitchState(Factory.WaitingForDefeatedEnemies());
        }
    }
}
