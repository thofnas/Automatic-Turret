using System;
using Events;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : Singleton<TurretScanner>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                GameEvents.OnEnemySpotted.Invoke(enemy);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                GameEvents.OnEnemyLost.Invoke(enemy);
            }
        }
    }
}

