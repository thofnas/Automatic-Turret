using System;
using System.Collections.Generic;
using _Managers;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : Singleton<TurretScanner>
    {
        public  EventHandler OnEnemySpotted;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                EnemyManager.Instance.AddEnemy(enemy);
                OnEnemySpotted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
