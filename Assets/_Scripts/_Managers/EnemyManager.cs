using System;
using System.Collections.Generic;
using System.Linq;
using _Events;
using UnityEngine;

namespace _Managers
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        private readonly Dictionary<Guid, Enemy> _enemiesInSightList = new();
        private readonly List<Enemy> _allEnemiesList = new();

        private void OnEnable()
        {
            GameEvents.OnEnemySpawned.AddListener(GameEvents_Enemy_OnEnemySpawned);
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.AddListener(GameEvents_Enemy_OnEnemySpotted);
            GameEvents.OnEnemyLost.AddListener(GameEvents_Enemy_OnEnemyLost);
        }

        private void OnDisable()
        {
            GameEvents.OnEnemySpawned.RemoveListener(GameEvents_Enemy_OnEnemySpawned);
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.RemoveListener(GameEvents_Enemy_OnEnemySpotted);
            GameEvents.OnEnemyLost.RemoveListener(GameEvents_Enemy_OnEnemyLost);
        }

        public void SpawnEnemy()
        {
            
        }
        
        #region Methods helpers for all enemies
        private void AddEnemyToList(Enemy enemy) => _allEnemiesList.Add(enemy);

        private void RemoveEnemyFromList(Enemy enemy) => _allEnemiesList.Remove(enemy);
        #endregion

        #region Methods helpers for enemies in sight
        public Enemy GetClosestSpottedEnemy()
        {
            Transform turret = GameManager.Instance.Turret.GetTransform();
            float closestDistance = float.PositiveInfinity;
            Enemy closestEnemy = null;

            foreach (var enemy in _enemiesInSightList)
            {
                float distance = Vector3.Distance(enemy.Value.transform.position, turret.position);
        
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.Value;
                }
            }
            
            return closestEnemy;
        }
        
        private bool TryGetSpottedEnemy(Guid enemyID, out Enemy enemy) => _enemiesInSightList.TryGetValue(enemyID, out enemy);

        private bool TryGetSpottedEnemy(Enemy enemy, out Guid enemyID)
        {
            if (!_enemiesInSightList.ContainsValue(enemy))
            {
                enemyID = Guid.Empty;
                return false;
            }

            enemyID = _enemiesInSightList.FirstOrDefault(x => x.Value == enemy).Key;
            return true;
        }

        private void AddEnemyToSpottedList(Enemy enemy) => _enemiesInSightList.Add(enemy.InstanceID, enemy);

        private void AddEnemyToSpottedList(Guid enemyID) => throw new NotImplementedException();

        private void RemoveEnemyFromSpottedList(Enemy enemy) => _enemiesInSightList.Remove(enemy.InstanceID);

        private void RemoveEnemyFromSpottedList(Guid enemyID) => _enemiesInSightList.Remove(enemyID);

        public bool HasEnemyInSight(Enemy enemy) => _enemiesInSightList.ContainsValue(enemy);

        public bool HasEnemyInSight(Guid enemyID) => _enemiesInSightList.ContainsKey(enemyID);

        public bool HasEnemyInSight() => _enemiesInSightList.Count > 0;
        #endregion

        #region Events methods
        private void GameEvents_Enemy_OnEnemyDestroyed(Guid enemyID)
        {
            TryGetSpottedEnemy(enemyID, out Enemy enemy);
            RemoveEnemyFromSpottedList(enemyID);
            RemoveEnemyFromList(enemy);
        }

        private void GameEvents_Enemy_OnEnemySpawned(Enemy enemy)
        {
            AddEnemyToList(enemy);
            print(_allEnemiesList.Count);
        }
        
        private void GameEvents_Enemy_OnEnemySpotted(Enemy enemy) => AddEnemyToSpottedList(enemy);

        private void GameEvents_Enemy_OnEnemyLost(Enemy enemy) => RemoveEnemyFromSpottedList(enemy);
        #endregion
    }
}