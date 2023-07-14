using System;
using System.Collections.Generic;
using _Events;
using Turret.StateMachine;
using UnityEngine;

namespace _Managers
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        public readonly Dictionary<Guid, Enemy> EnemiesInSightList = new();

        private void OnEnable()
        {
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.AddListener(GameEvents_Enemy_OnEnemySpotted);
        }
        private void GameEvents_Enemy_OnEnemySpotted(Enemy enemyId)
        {
            AddEnemyToList(enemyId);
        }

        public Enemy GetClosestEnemy()
        {
            print(TurretStateMachine.Instance);
            Transform turret = TurretStateMachine.Instance.GetTransform();
            float closestDistance = float.PositiveInfinity;
            Enemy closestEnemy = null;

            foreach (var enemy in EnemiesInSightList)
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

        public void AddEnemyToList(Enemy enemy) => EnemiesInSightList.Add(enemy.InstanceID, enemy);

        public void RemoveEnemyFromList(Enemy enemy) => EnemiesInSightList.Remove(enemy.InstanceID);

        public void RemoveEnemyFromList(Guid enemyID) => EnemiesInSightList.Remove(enemyID);

        private void GameEvents_Enemy_OnEnemyDestroyed(Guid enemyID) => RemoveEnemyFromList(enemyID);
    }
}