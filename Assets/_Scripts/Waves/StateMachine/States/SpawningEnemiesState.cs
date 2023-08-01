using System.Collections;
using Managers;

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
            _spawnDelay = Ctx.GetCurrentWaveData().SpawnDelay;
            _prefab = Ctx.GetCurrentWaveData().EnemiesData[0].EnemyPrefab;
            
            _spawnEnemiesRoutine =
                EnemyManager.SpawnEnemiesRoutine(_prefab, _spawnDelay, Ctx.EnemiesToSpawnCount);
            
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
