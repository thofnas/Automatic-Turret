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

        private void OnEnable()
        {
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.AddListener(GameEvents_Enemy_OnEnemySpotted);
        }
        
        private void OnDisable()
        {
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.RemoveListener(GameEvents_Enemy_OnEnemySpotted);
        }
        
        #region Methods helpers
        public Enemy GetClosestEnemy()
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
        
        public bool TryGetEnemy(Guid enemyID, out Enemy enemy) => _enemiesInSightList.TryGetValue(enemyID, out enemy);

        public bool TryGetEnemy(Enemy enemy, out Guid enemyID)
        {
            if (!_enemiesInSightList.ContainsValue(enemy))
            {
                enemyID = Guid.Empty;
                return false;
            }

            enemyID = _enemiesInSightList.FirstOrDefault(x => x.Value == enemy).Key;
            return true;
        }

        public void AddEnemyToList(Enemy enemy) => _enemiesInSightList.Add(enemy.InstanceID, enemy);

        public void AddEnemyToList(Guid enemyID) => throw new NotImplementedException();

        public void RemoveEnemyFromList(Enemy enemy) => _enemiesInSightList.Remove(enemy.InstanceID);

        public void RemoveEnemyFromList(Guid enemyID) => _enemiesInSightList.Remove(enemyID);

        public bool HasEnemyInSight(Enemy enemy) => _enemiesInSightList.ContainsValue(enemy);

        public bool HasEnemyInSight(Guid enemyID) => _enemiesInSightList.ContainsKey(enemyID);

        public bool HasEnemyInSight() => _enemiesInSightList.Count > 0;
        #endregion

        #region Events methods
        private void GameEvents_Enemy_OnEnemyDestroyed(Guid enemyID) => RemoveEnemyFromList(enemyID);

        private void GameEvents_Enemy_OnEnemySpotted(Enemy enemy) => AddEnemyToList(enemy);
        #endregion
    }
}