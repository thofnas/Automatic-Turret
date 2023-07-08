using System;
using System.Collections.Generic;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : Singleton<TurretScanner>
    {
        public static EventHandler OnEnemySpotted;
        public readonly Queue<Transform> EnemyList = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Enemy>() != null)
            {
                EnemyList.Enqueue(other.transform);
                OnEnemySpotted?.Invoke(this, EventArgs.Empty);
            }
        }
        
        private void OnEnable()
        {
            Enemy.OnDestroyEvent += Enemy_OnDestroyEvent;
        }
    
        private void OnDisable()
        {
            Enemy.OnDestroyEvent -= Enemy_OnDestroyEvent;
        }
    
        private void Enemy_OnDestroyEvent()
        {
            EnemyList.Dequeue();
        }
    }
}
