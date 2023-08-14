using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        public const float MIN_DISTANCE_TO_SPAWN_ENEMY = 1f;
        public const float MAX_DISTANCE_TO_SPAWN_ENEMY = 3f;
        
        private readonly Dictionary<Guid, Enemy.Enemy> _enemiesInSightList = new();
        private readonly List<Enemy.Enemy> _allEnemiesList = new();

        public void Initialize()
        {
            GameEvents.OnEnemySpawned.AddListener(GameEvents_Enemy_OnEnemySpawned);
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.AddListener(GameEvents_Enemy_OnEnemySpotted);
            GameEvents.OnEnemyLostFromView.AddListener(GameEvents_Enemy_OnLostFromView);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
        }

        private void OnDestroy()
        {
            GameEvents.OnEnemySpawned.RemoveListener(GameEvents_Enemy_OnEnemySpawned);
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.RemoveListener(GameEvents_Enemy_OnEnemySpotted);
            GameEvents.OnEnemyLostFromView.RemoveListener(GameEvents_Enemy_OnLostFromView);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnEnded);
        }

        public void SpawnEnemy(Enemy.Enemy enemyPrefab, Vector3 spawnPosition, Quaternion rotation)
        {
            spawnPosition.y = 0 + enemyPrefab.transform.localScale.y / 2;

            Enemy.Enemy enemy = Instantiate(enemyPrefab, spawnPosition, rotation);
            GameEvents.OnEnemySpawned.Invoke(enemy);
        }

        private void ClearAllEnemies()
        {
            foreach (Enemy.Enemy enemy in _allEnemiesList)
            {
                Destroy(enemy.gameObject);  
            }
            _allEnemiesList.Clear();
            _enemiesInSightList.Clear();
        }

        #region Methods helpers for all enemies
        private void AddEnemyToList(Enemy.Enemy enemy) => _allEnemiesList.Add(enemy);

        private void RemoveEnemyFromList(Enemy.Enemy enemy) => _allEnemiesList.Remove(enemy);

        public bool IsAnyEnemyExists() => _allEnemiesList.Count > 0;
        #endregion

        #region Methods helpers for enemies in sight
        public Enemy.Enemy GetClosestSpottedEnemy()
        {
            Transform turret = GameManager.Instance.TurretStateMachine.GetTransform();
            float closestDistance = float.PositiveInfinity;
            Enemy.Enemy closestEnemy = null;

            foreach (var enemy in _enemiesInSightList)
            {
                if (enemy.Value == null) continue;
                
                float distance = Vector3.Distance(enemy.Value.transform.position, turret.position);
        
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.Value;
                }
            }
            
            return closestEnemy;
        }
        
        private bool TryGetSpottedEnemy(Guid enemyID, out Enemy.Enemy enemy) => _enemiesInSightList.TryGetValue(enemyID, out enemy);

        private bool TryGetSpottedEnemy(Enemy.Enemy enemy, out Guid enemyID)
        {
            if (!_enemiesInSightList.ContainsValue(enemy))
            {
                enemyID = Guid.Empty;
                return false;
            }

            enemyID = _enemiesInSightList.FirstOrDefault(x => x.Value == enemy).Key;
            return true;
        }

        private void AddEnemyToSpottedList(Enemy.Enemy enemy) => _enemiesInSightList.Add(enemy.InstanceID, enemy);

        private void AddEnemyToSpottedList(Guid enemyID) => throw new NotImplementedException();

        private void RemoveEnemyFromSpottedList(Enemy.Enemy enemy) => _enemiesInSightList.Remove(enemy.InstanceID);

        private void RemoveEnemyFromSpottedList(Guid enemyID) => _enemiesInSightList.Remove(enemyID);

        public bool HasEnemyInSight(Enemy.Enemy enemy) => _enemiesInSightList.ContainsValue(enemy);

        public bool HasEnemyInSight(Guid enemyID) => _enemiesInSightList.ContainsKey(enemyID);

        public bool HasEnemyInSight() => _enemiesInSightList.Count > 0;
        #endregion

        #region Events methods
        private void GameEvents_Enemy_OnEnemyDestroyed(Enemy.Enemy enemy)
        {
            RemoveEnemyFromSpottedList(enemy);
            RemoveEnemyFromList(enemy);
        }

        private void GameEvents_Enemy_OnEnemySpawned(Enemy.Enemy enemy) => AddEnemyToList(enemy);

        private void GameEvents_Enemy_OnEnemySpotted(Enemy.Enemy enemy) => AddEnemyToSpottedList(enemy);

        private void GameEvents_Enemy_OnLostFromView(Enemy.Enemy enemy) => RemoveEnemyFromSpottedList(enemy);
        
        private void GameEvents_Wave_OnEnded() => ClearAllEnemies();
        #endregion
    }
}